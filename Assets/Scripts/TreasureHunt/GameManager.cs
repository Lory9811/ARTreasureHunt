using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Vuforia;

public class GameManager : MonoBehaviour {
    private int currentTreasure = 0;

    void Start() {
        VuforiaBehaviour.Instance.VideoBackground.StartVideoBackgroundRendering();
    }

    public void StartChallenge(int level, string hint) {
        Debug.Log(level);
    }
}
