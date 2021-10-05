using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationService : MonoBehaviour {
    IEnumerator Start() {
        if (!Input.location.isEnabledByUser) {
            yield break;
        }

        Input.location.Start(0.5f, 1.0f);

        int timeout = 10;
        while (Input.location.status == LocationServiceStatus.Initializing) {
            yield return new WaitForSeconds(1);
            timeout--;
        }

        if (timeout < 1 ||
            Input.location.status == LocationServiceStatus.Failed) {
            yield break;
        }
    }

    void FixedUpdate() {
        /*
        Debug.Log("location: " + Input.location.lastData.latitude + " " +
             Input.location.lastData.longitude + " " + 
             Input.location.lastData.altitude + " " + 
             Input.location.lastData.horizontalAccuracy + " " + 
             Input.location.lastData.timestamp);*/
    }
}
