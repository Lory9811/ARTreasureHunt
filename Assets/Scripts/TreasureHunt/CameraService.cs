using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraService : MonoBehaviour {
    private WebCamTexture webcamTexture;
    private Texture2D webcamImage;

    void Start() {
        /*WebCamDevice[] webCamDevices = WebCamTexture.devices;
        foreach (var device in webCamDevices) {
            Debug.Log(device.name);
        }*/

        webcamTexture = new WebCamTexture();
        webcamTexture.Play();


        webcamImage = new Texture2D(webcamTexture.height, webcamTexture.width);
        webcamImage.UpdateExternalTexture(webcamTexture.GetNativeTexturePtr());
    }

    void FixedUpdate() {
    }

    public WebCamTexture GetCameraTexture() {
        return webcamTexture;
    }
}
