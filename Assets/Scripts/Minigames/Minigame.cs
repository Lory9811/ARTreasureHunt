using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame : MonoBehaviour {
    public delegate void CompletionCallback(bool success);
    public delegate void CleanupCallback();

    private CompletionCallback completionCallback;
    private CleanupCallback cleanupCallback;
    private string hint;
    private GameManager gameManager;

    void Start() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void SetHintText(string hint) {
        this.hint = hint;
    }

    public string GetHintText() {
        return hint;
    }

    public void SetCompletionCallback(CompletionCallback callback) {
        completionCallback = callback;
    }

    public void SetCleanupCallback(CleanupCallback callback) {
        cleanupCallback = callback;
    }

    public void CompleteGame(bool success) {
        completionCallback(success);
    }

    public void Cleanup() {
        cleanupCallback();
    }
}
