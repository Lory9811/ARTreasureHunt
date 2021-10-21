using UnityEngine;
using UnityEngine.Events;
using Vuforia;

public class TrackedImageBehaviour : DefaultObserverEventHandler {
    public delegate void DetectionCallback();

    private DetectionCallback callback = null;

    protected override void HandleTargetStatusChanged(Status previousStatus, Status newStatus) {
        if (newStatus == Status.TRACKED && previousStatus != newStatus) {
            if (callback != null) callback();
        }
    }

    public void SetDetectionCallback(DetectionCallback callback) {
        this.callback = callback;
    }
}
