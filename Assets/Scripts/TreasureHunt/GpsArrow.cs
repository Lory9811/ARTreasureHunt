using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GpsArrow : MonoBehaviour {
    Gyroscope gyro; // TODO: move to location service

    void Start() {
        gyro = Input.gyro;
        gyro.enabled = true;
    }

    void FixedUpdate() {
        //transform.rotation = Quaternion.Euler(-gyro.attitude.eulerAngles.x, gyro.attitude.eulerAngles.y, -gyro.attitude.eulerAngles.z);
    }
}
