using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Vuforia;

public class TreasureHunt : MonoBehaviour {
    [System.Serializable]
    private class HuntDescriptor {
        public string format;
        public TreasureDescriptor[] treasures;
    }

    [System.Serializable]
    private class TreasureDescriptor {
        public string type;
        public string url;
        public string hint;
    }

    private class Treasure {
        string hint;
        ImageTargetBehaviour target;
        int id;

        public delegate void DetectionCallback(Treasure treasure);

        public Treasure(int id) {
            this.id = id;
        }

        public string GetName() {
            return target.gameObject.name;
        }

        public int GetId() {
            return id;
        }

        public string GetHint() {
            return hint;
        }

        public IEnumerator DownloadTreasure(string path, GameServer server, TrackedImageBehaviour.DetectionCallback callback) {
            Debug.Log(server.GetImageLocation(path));
            using (var request = UnityWebRequestTexture.GetTexture(server.GetImageLocation(path))) {
                yield return request.SendWebRequest();

                switch (request.result) {
                    case UnityWebRequest.Result.Success:
                        target = VuforiaBehaviour.Instance.ObserverFactory.CreateImageTarget(
                            DownloadHandlerTexture.GetContent(request), 0.10f, path);
                        var handler = target.gameObject.AddComponent<TrackedImageBehaviour>();
                        handler.SetDetectionCallback(callback);
                        Debug.Log("Treasure downloaded " + path);
                        break;
                    default:
                        Debug.LogError(request.error);
                        break;
                }
            }
        }

        public IEnumerator DownloadHint(string path, GameServer server) {
            Debug.Log(server.GetHintLocation(path));
            using (var request = UnityWebRequest.Get(server.GetHintLocation(path))) {
                yield return request.SendWebRequest();

                switch (request.result) {
                    case UnityWebRequest.Result.Success:
                        hint = request.downloadHandler.text;
                        Debug.Log("Hint downloaded " + path);
                        break;
                    default:
                        Debug.LogError(request.error);
                        break;
                }
            }
        }
    }
    
    private Treasure[] treasures;

    private GameManager gameManager;

    void Start() {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) => {
            var manager = GameObject.Find("GameManager")?.GetComponent<GameManager>();
            if (manager != null) {
                gameManager = manager;
            }
        };
    }

    public void LoadHunt(string id, GameServer server) {
        StartCoroutine(DoLoad(id, server));
    }

    private void OnTreasureSeen(Treasure treasure) {
        Debug.Log("Found treasure " + treasure.GetName());
        Handheld.Vibrate();
        gameManager?.StartChallenge(treasure.GetId(), treasure.GetHint());
    }

    private IEnumerator DoLoad(string id, GameServer server) {
        yield return SceneManager.LoadSceneAsync("TreasureHunt", LoadSceneMode.Single);
        yield return DownloadHunt(id, server);
    }

    private IEnumerator DownloadHunt(string path, GameServer server) {
        using (var request = UnityWebRequest.Get(server.GetHuntDescriptorLocation(path))) {
            yield return request.SendWebRequest();

            switch (request.result) {
                case UnityWebRequest.Result.Success:
                    var huntDescriptor = JsonUtility.FromJson<HuntDescriptor>(request.downloadHandler.text);
                    var treasuresList = new List<Treasure>();
                    for (var i = 0; i != huntDescriptor.treasures.Length; i++) {
                        //foreach (var treasure in huntDescriptor.treasures) {
                        var treasure = huntDescriptor.treasures[i];
                        var tempTreasure = new Treasure(i);
                        treasuresList.Add(tempTreasure);
                        Debug.Log("Downloading treasure " + i);
                        yield return StartCoroutine(tempTreasure.DownloadTreasure(treasure.url, server, () => OnTreasureSeen(tempTreasure)));
                        yield return StartCoroutine(tempTreasure.DownloadHint(treasure.hint, server));
                        Debug.Log("Finished downloading " + i);
                    }
                    treasures = treasuresList.ToArray();
                    break;
                default:
                    Debug.LogError(request.error);
                    break;
            }
        }
    }

    private IEnumerator SwitchScene() {
        var load = SceneManager.LoadSceneAsync("TreasureHunt", LoadSceneMode.Single);
        load.allowSceneActivation = false;

        while (!load.isDone) {
            yield return null;
        }

        load.allowSceneActivation = true;
    }
}
