using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour {
    public void StartGame() {
        var huntDataObject = new GameObject("HuntData");
        var huntData = huntDataObject.AddComponent<TreasureHunt>();
        huntData.LoadHunt("0", new LocalServer());
        SceneManager.LoadSceneAsync("TreasureHunt", LoadSceneMode.Single);
    }

    public void CreateGame() {

    }
}
