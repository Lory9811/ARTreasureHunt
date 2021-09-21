using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.InteropServices;

public class TreasureHunt : MonoBehaviour {
    public TextMeshProUGUI text;

    void Start() {
        text.SetText("{0}.{1}", Vision.GetVersion().Item1, Vision.GetVersion().Item2);
    }

    void Update() {
        
    }
}
