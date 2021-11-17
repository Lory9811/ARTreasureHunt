using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Vuforia;

public class GameManager : MonoBehaviour {
    public GameObject[] MinigamesList;
    public GameObject HintPrompt;
    public TextMeshProUGUI ScoreText;

    private Dictionary<string, GameObject> minigames;
    private int currentTreasure = 0;
    private int score = 0;
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

    private void AddPoints(int points) {
        score += points;
        if (ScoreText != null) ScoreText.text = (score * 15).ToString();
    }

    public void StartChallenge(int level, string hint, string minigame) {
        if (pauseDetection || level != currentTreasure) return;
        pauseDetection = true;
        var game = Instantiate(minigames[minigame.ToLower()]).GetComponent<Minigame>();
        game.SetCompletionCallback((bool status) => {
            Debug.Log("Completed game " + level + " " + status);
            currentTreasure++;
            AddPoints(1);
            DisplayHint(hint, () => {
                game.Cleanup();
            });
        });
    }
}
