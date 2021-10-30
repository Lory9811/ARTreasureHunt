using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public abstract class GameServer {
    public delegate void DownloadCallback<T>(T result);

    protected abstract string GetHuntDescriptorLocation(string id);
    protected abstract string GetImageLocation(string url);
    protected abstract string GetHintLocation(string url);

    public virtual IEnumerator DownloadDescriptor(string id, DownloadCallback<TreasureHunt.HuntDescriptor> callback) {
        using (var request = UnityWebRequest.Get(GetHuntDescriptorLocation(id))) {
            yield return request.SendWebRequest();

            switch (request.result) {
                case UnityWebRequest.Result.Success:
                    var huntDescriptor = JsonUtility.FromJson<TreasureHunt.HuntDescriptor>(request.downloadHandler.text);
                    callback(huntDescriptor);
                    break;
                default:
                    Debug.LogError(request.error);
                    break;
            }
        }
    }

    public virtual IEnumerator DownloadImage(string url, DownloadCallback<Texture2D> callback) {
        using (var request = UnityWebRequestTexture.GetTexture(GetImageLocation(url))) {
            yield return request.SendWebRequest();

            switch (request.result) {
                case UnityWebRequest.Result.Success:
                    callback(DownloadHandlerTexture.GetContent(request));
                    break;
                default:
                    Debug.LogError(request.error);
                    break;
            }
        }
    }

    public virtual IEnumerator DownloadHint(string url, DownloadCallback<string> callback) {
        using (var request = UnityWebRequest.Get(GetHintLocation(url))) {
            yield return request.SendWebRequest();

            switch (request.result) {
                case UnityWebRequest.Result.Success:
                    var hint = request.downloadHandler.text;
                    callback(hint);
                    break;
                default:
                    Debug.LogError(request.error);
                    break;
            }
        }
    }
}

public class LocalServer : GameServer {
    protected override string GetHintLocation(string url) {
        return Path.Combine(Application.streamingAssetsPath, url);
    }

    protected override string GetHuntDescriptorLocation(string id) {
        return Path.Combine(Application.streamingAssetsPath, "hunts", id, "descriptor");
    }

    protected override string GetImageLocation(string url) {
        return Path.Combine(Application.streamingAssetsPath, url);
    }
}

public class RemoteServer : GameServer {
    protected override string GetHintLocation(string url) {
        throw new System.NotImplementedException();
    }

    protected override string GetHuntDescriptorLocation(string id) {
        throw new System.NotImplementedException();
    }

    protected override string GetImageLocation(string url) {
        throw new System.NotImplementedException();
    }
}
