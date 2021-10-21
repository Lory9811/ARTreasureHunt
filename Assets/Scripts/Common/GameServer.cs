using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public abstract class GameServer {
    public abstract string GetHuntDescriptorLocation(string id);
    public abstract string GetImageLocation(string url);
    public abstract string GetHintLocation(string url);
}

public class LocalServer : GameServer {
    public override string GetHintLocation(string url) {
        return Path.Combine(Application.streamingAssetsPath, url);
    }

    public override string GetHuntDescriptorLocation(string id) {
        return Path.Combine(Application.streamingAssetsPath, "hunts", id, "descriptor");
    }

    public override string GetImageLocation(string url) {
        return Path.Combine(Application.streamingAssetsPath, url);
    }
}

public class RemoteServer : GameServer {
    public override string GetHintLocation(string url) {
        throw new System.NotImplementedException();
    }

    public override string GetHuntDescriptorLocation(string id) {
        throw new System.NotImplementedException();
    }

    public override string GetImageLocation(string url) {
        throw new System.NotImplementedException();
    }
}
