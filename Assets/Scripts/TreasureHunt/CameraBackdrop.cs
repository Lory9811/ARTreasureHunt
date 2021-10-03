using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBackdrop : MonoBehaviour {
    public CameraService cameraService;
    public Shader blendShader;
    public Shader fitShader;

    private Material blendMaterial;
    private Material fitMaterial;

    void Start() {
        fitMaterial = new Material(fitShader);
        blendMaterial = new Material(blendShader);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        Camera.main.targetTexture = null;
        Texture2D cameraTexture = cameraService.GetCameraTexture();
        fitMaterial.SetFloat("_AspectRatio", (float)Screen.width / Screen.height);
        fitMaterial.SetFloat("_SrcAspectRatio", (float)cameraTexture.width / cameraTexture.height);
        Graphics.Blit(cameraTexture, destination, fitMaterial);
        Graphics.Blit(source, destination, blendMaterial);
    }
}
