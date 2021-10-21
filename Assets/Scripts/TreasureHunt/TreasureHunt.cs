using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
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

        public delegate void DetectionCallback(Treasure treasure);

        /* Needed for coroutines */
        public Treasure(TreasureHunt hunt, TreasureDescriptor descriptor, GameServer server, DetectionCallback callback) {
            hunt.StartCoroutine(DownloadTreasure(descriptor.url, server, () => callback(this)));
            hunt.StartCoroutine(DownloadHint(descriptor.hint, server));
        }

        public string GetName() {
            return target.gameObject.name;
        } 

        private IEnumerator DownloadTreasure(string path, GameServer server, TrackedImageBehaviour.DetectionCallback callback) {
            Debug.Log(server.GetImageLocation(path));
            using (var request = UnityWebRequestTexture.GetTexture(server.GetImageLocation(path))) {
                yield return request.SendWebRequest();

                switch (request.result) {
                    case UnityWebRequest.Result.Success:
                        target = VuforiaBehaviour.Instance.ObserverFactory.CreateImageTarget(
                            DownloadHandlerTexture.GetContent(request), 0.10f, path);
                        var handler = target.gameObject.AddComponent<TrackedImageBehaviour>();
                        handler.SetDetectionCallback(callback);
                        break;
                    default:
                        Debug.LogError(request.error);
                        break;
                }
            }
        }

        private IEnumerator DownloadHint(string path, GameServer server) {
            Debug.Log(server.GetHintLocation(path));
            using (var request = UnityWebRequest.Get(server.GetHintLocation(path))) {
                yield return request.SendWebRequest();

                switch (request.result) {
                    case UnityWebRequest.Result.Success:
                        hint = request.downloadHandler.text;
                        break;
                    default:
                        Debug.LogError(request.error);
                        break;
                }
            }
        }
    }
    
    private Treasure[] treasures;

    void Awake() {
        DontDestroyOnLoad(gameObject);        
    }

    public void LoadHunt(string id, GameServer server) {
        StartCoroutine(DownloadHunt(id, server));
    }

    private void OnTreasureSeen(Treasure treasure) {
        Debug.Log("Found treasure " + treasure.GetName());
        Handheld.Vibrate();
    }

    private IEnumerator DownloadHunt(string path, GameServer server) {
        using (var request = UnityWebRequest.Get(server.GetHuntDescriptorLocation(path))) {
            yield return request.SendWebRequest();

            switch (request.result) {
                case UnityWebRequest.Result.Success:
                    var huntDescriptor = JsonUtility.FromJson<HuntDescriptor>(request.downloadHandler.text);
                    var treasuresList = new List<Treasure>();
                    foreach (var treasure in huntDescriptor.treasures) {
                        treasuresList.Add(new Treasure(this, treasure, server, OnTreasureSeen));
                    }
                    break;
                default:
                    Debug.LogError(request.error);
                    break;
            }
        }
    }
}
