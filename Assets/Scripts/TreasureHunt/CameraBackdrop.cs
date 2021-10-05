using UnityEngine;
using OpenCvSharp;

public class CameraBackdrop : MonoBehaviour {
    public CameraService cameraService;
    public Shader blendShader;
    public Shader fitShader;

    public Texture2D test;

    private Material blendMaterial;
    private Material fitMaterial;

    private Texture2D tmp;

    void Start() {
        fitMaterial = new Material(fitShader);
        blendMaterial = new Material(blendShader);
        tmp = Texture2D.whiteTexture;
    }

    private void FixedUpdate() {
        WebCamTexture cameraTexture = cameraService.GetCameraTexture();
        Mat mat = OpenCvSharp.Unity.TextureToMat(cameraTexture);
        Mat grayscaleMat = new Mat();
        Cv2.CvtColor(mat, grayscaleMat, ColorConversionCodes.RGBA2GRAY);

        tmp = OpenCvSharp.Unity.MatToTexture(grayscaleMat);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        Camera.main.targetTexture = null;

        fitMaterial.SetFloat("_AspectRatio", (float)Screen.width / Screen.height);
        fitMaterial.SetFloat("_SrcAspectRatio", (float)tmp.width / tmp.height);

        Graphics.Blit(tmp, destination, fitMaterial);
        Graphics.Blit(source, destination, blendMaterial);
    }
}
