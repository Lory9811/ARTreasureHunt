using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

[RequireComponent(typeof(Minigame))]
public class Mastermind : MonoBehaviour {
    private Minigame minigame;

    void Start() {
        minigame = gameObject.GetComponent<Minigame>();
        minigame.SetCleanupCallback(() => {
            Destroy(gameObject);
        });
        VuforiaBehaviour.Instance.VideoBackground.StopVideoBackgroundRendering();
    }

    public void Button() {
        minigame.CompleteGame(true);
        VuforiaBehaviour.Instance.VideoBackground.StartVideoBackgroundRendering();
    }
}
