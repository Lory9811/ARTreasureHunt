using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class GameManager : MonoBehaviour {
    public GameObject[] MinigamesList;
    public GameObject HintPrompt;

    private Dictionary<string, GameObject> minigames;
    private int currentTreasure = 0;
    private bool inChallenge = false;

    void Start() {
        VuforiaBehaviour.Instance.VideoBackground.StartVideoBackgroundRendering();
        minigames = new Dictionary<string, GameObject>();

        foreach (var minigame in MinigamesList) {
            minigames.Add(minigame.name.ToLower(), minigame);
        }
    }

    public void StartChallenge(int level, string hint, string minigame) {
        Debug.Log(level);
        if (level != currentTreasure || inChallenge) return;
        inChallenge = true;
        Debug.Log(minigame.ToLower());
        var game = Instantiate(minigames[minigame.ToLower()]).GetComponent<Minigame>();
        game.SetCompletionCallback((bool status) => {
            Debug.Log("Completed game " + level + " " + status);
            currentTreasure++;
            HintPrompt.GetComponent<HintPrompt>().SetHintText(hint);
            HintPrompt.SetActive(true);
            HintPrompt.GetComponent<HintPrompt>().SetConfirmCallback(() => {
                inChallenge = false;
                game.Cleanup();
            });
        });
    }
}
