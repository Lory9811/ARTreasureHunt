using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureHunt : MonoBehaviour {
    public CameraService cameraService;
    public Texture2D testObject;

    private void FixedUpdate() {
        // Debug.Log(Vision.DetectObject(cameraService.GetCameraTexture(), testObject));
    }
}
