using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Vuforia;

public class TreasureHunt : MonoBehaviour {
    /*public void Start() {
        VuforiaApplication.Instance.OnVuforiaStarted += () => {
            Debug.Log("Vuforia started");
            Texture2D testObject = 
            var target = VuforiaBehaviour.Instance.ObserverFactory.CreateImageTarget(testObject, 0.05f, "test");
            target.gameObject.AddComponent<DefaultObserverEventHandler>();
            target.gameObject.GetComponent<DefaultObserverEventHandler>().OnTargetFound.AddListener(() => {
                Debug.Log("Target found");
                Handheld.Vibrate();
            });
            target.enabled = true;
        };
        //VuforiaApplication.Instance.Initialize();
    }*/

    void Start() {
        StartCoroutine(CreateImageTargetFromDownloadedTexture());
    }

    IEnumerator CreateImageTargetFromDownloadedTexture() {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(Path.Combine(Application.streamingAssetsPath, "TestTrackers/tracker_0_scaled.jpg"))) {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError) {
                Debug.Log(uwr.error);
            } else {
                var target = VuforiaBehaviour.Instance.ObserverFactory.CreateImageTarget(DownloadHandlerTexture.GetContent(uwr), 0.10f, "test");
                DefaultObserverEventHandler handler = target.gameObject.AddComponent<TrackedImageBehaviour>();
            }
        }
    }

    public void TestHandler() {
        Debug.Log("Target found");
        Handheld.Vibrate();
    }

    public void OnDestroy() {
        //VuforiaApplication.Instance.Deinit();
    }

    public void FixedUpdate() {
        // Debug.Log(Vision.DetectObject(cameraService.GetCameraTexture(), testObject));
    }
}
