using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class Mastermind : Minigame {
    public Texture2DArray buttons;

    public override void Init() {
        VuforiaBehaviour.Instance.VideoBackground.StopVideoBackgroundRendering();
    }
}
