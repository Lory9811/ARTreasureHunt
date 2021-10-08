using UnityEngine;
using UnityEngine.Events;
using Vuforia;

public class TrackedImageBehaviour : DefaultObserverEventHandler {
    protected override void HandleTargetStatusChanged(Status previousStatus, Status newStatus) {
        if (newStatus == Status.TRACKED && previousStatus != newStatus) {
            Debug.Log("Tracker detected");
            Debug.Log(gameObject.name);
            Handheld.Vibrate();
        }
    }
}
