using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class GameManager : MonoBehaviour {
    public GameObject[] MinigamesList;
    public GameObject HintPrompt;

    private Dictionary<string, GameObject> minigames;
    private int currentTreasure = 0;
    private bool pauseDetection = false;

    void Start() {
        VuforiaBehaviour.Instance.VideoBackground.StartVideoBackgroundRendering();
        minigames = new Dictionary<string, GameObject>();

        foreach (var minigame in MinigamesList) {
            minigames.Add(minigame.name.ToLower(), minigame);
        }
    }

    public void DisplayHint(string hint, HintPrompt.ConfirmCallback confirmCallback = null) {
        pauseDetection = true;
        HintPrompt.GetComponent<HintPrompt>().SetHintText(hint);
        HintPrompt.GetComponent<HintPrompt>().SetConfirmCallback(() => { pauseDetection = false; } + confirmCallback);
        HintPrompt.SetActive(true);
    }

    public void StartChallenge(int level, string hint, string minigame) {
        Debug.Log(level);
        if (level != currentTreasure || pauseDetection) return;
        pauseDetection = true;
        Debug.Log(minigame.ToLower());
        var game = Instantiate(minigames[minigame.ToLower()]).GetComponent<Minigame>();
        game.SetCompletionCallback((bool status) => {
            Debug.Log("Completed game " + level + " " + status);
            currentTreasure++;
            DisplayHint(hint, () => {
                game.Cleanup();
            });
        });
    }
}
