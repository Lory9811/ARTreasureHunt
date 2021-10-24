using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vuforia;

public class TitleScreen : MonoBehaviour {
    void Start() {
        DontDestroyOnLoad(GameObject.Find("ARCamera"));
        VuforiaApplication.Instance.OnVuforiaStarted += () => {
            VuforiaBehaviour.Instance.VideoBackground.StopVideoBackgroundRendering();
        };
    }

    public void StartGame() {
        var huntDataObject = new GameObject("HuntData");
        var huntData = huntDataObject.AddComponent<TreasureHunt>();
        huntData.LoadHunt("0", new LocalServer());
    }

    public void CreateGame() {

    }
}
