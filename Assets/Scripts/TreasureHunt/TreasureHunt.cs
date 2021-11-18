using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vuforia;

public class TreasureHunt : MonoBehaviour {
    /**
     * Temporary class for deserialization
     */
    [System.Serializable]
    public class HuntDescriptor {
        public string format;
        public string hint;
        public TreasureDescriptor[] treasures;
    }

    /**
     * Temporary class for deserialization
     */
    [System.Serializable]
    public class TreasureDescriptor {
        public string type;
        public string url;
        public string hint;
        public string minigame;
    }

    private class Treasure {
        public string Hint {
            get;
            private set;
        }

        public int Id {
            get;
            private set;
        }

        public string Name => target.gameObject.name;

        public string Minigame {
            get;
            private set;
        }

        public string TreasureType {
            get;
            private set;
        }

        ImageTargetBehaviour target;

        public delegate void DetectionCallback(Treasure treasure);

        public Treasure(int id) {
            Id = id;
        }

        public IEnumerator Init(TreasureDescriptor descriptor, GameServer server, DetectionCallback callback) {
            TreasureType = descriptor.type;
            Minigame = descriptor.minigame;
            yield return server.DownloadImage(descriptor.url, (Texture2D texture) => {
                target = VuforiaBehaviour.Instance.ObserverFactory.CreateImageTarget(
                    texture, 0.10f, "Treasure_" + Id);
                var handler = target.gameObject.AddComponent<TrackedImageBehaviour>();
                handler.callback += () => callback(this);
            });
            yield return server.DownloadHint(descriptor.hint, (string hint) => {
                Hint = hint;
            });
        }
    }

    private string hint;
    private Treasure[] treasures;
    private GameManager gameManager;

    void Start() {
        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) => {
            var manager = GameObject.Find("GameManager")?.GetComponent<GameManager>();
            if (manager != null) {
                gameManager = manager;
            }
        };
    }

    /**
     * Download hunt data and switch to scene
     * @param id The hunt's id
     * @param server The data source server
     */
    public void LoadHunt(string id, GameServer server) {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(DoLoad(id, server));
    }

    /**
     * Callback for treasure detection
     * @param treasure The detected treasure
     */
    private void OnTreasureSeen(Treasure treasure) {
        Debug.Log("Found treasure " + treasure.Name);
        if (treasure.TreasureType == "final") {
            gameManager?.EndGame(treasure.Id);
        } else {
            gameManager?.StartChallenge(treasure.Id, treasure.Hint, "mastermind");
        }
    }

    /**
     * Treasure hunt download and scene load coroutine
     * @param id The hunt's id
     * @param server The data source server
     */
    private IEnumerator DoLoad(string id, GameServer server) {
        yield return SceneManager.LoadSceneAsync("TreasureHunt", LoadSceneMode.Single);
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());

        yield return server.DownloadDescriptor(id, (HuntDescriptor descriptor) => {
            StartCoroutine(server.DownloadHint(descriptor.hint, (string text) => {
                hint = text;
                gameManager.DisplayHint(hint);
            }));
            var treasuresList = new List<Treasure>();
            Debug.Log(treasuresList);
            Debug.Log(descriptor);
            for (var i = 0; i != descriptor.treasures.Length; i++) {
                var treasure = descriptor.treasures[i];
                Debug.Log(treasure);
                var tempTreasure = new Treasure(i);
                treasuresList.Add(tempTreasure);
                StartCoroutine(tempTreasure.Init(treasure, server, OnTreasureSeen));
            }
            treasures = treasuresList.ToArray();
        });
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
