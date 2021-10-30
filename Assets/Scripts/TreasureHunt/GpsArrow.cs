using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GpsArrow : MonoBehaviour {
    private class PolarPosition {
        public float latitude = 0.0f;
        public float longitude = 0.0f;
    }

    private const float EarthRadius = 6.3781E+06f;
    private const float SlowUpdatePeriod = 30.0f;

    private Gyroscope gyro; // TODO: move to location service
    private LocationService locationService;
    private PolarPosition target;

    void Start() {
        gyro = Input.gyro;
        gyro.enabled = true;
        locationService = GameObject.Find("GameManager").GetComponent<LocationService>();

        StartCoroutine(SlowUpdate());
    }

    private IEnumerator SlowUpdate() {
        while (true) {
            var startTime = Time.time;
            var currentPosition = locationService.GetLocationInfo();
            var bearing = Mathf.Rad2Deg * Mathf.Atan2(
                    Mathf.Cos(Mathf.Deg2Rad * target.latitude) * Mathf.Sin(Mathf.Deg2Rad * (target.longitude - currentPosition.longitude)),
                    Mathf.Cos(Mathf.Deg2Rad * currentPosition.latitude) * Mathf.Sin(Mathf.Deg2Rad * target.latitude) -
                        Mathf.Sin(Mathf.Deg2Rad * currentPosition.latitude) * Mathf.Cos(Mathf.Deg2Rad * target.latitude) * 
                        Mathf.Cos(Mathf.Deg2Rad * (target.longitude - currentPosition.longitude))
                );

            transform.rotation = Quaternion.Euler(0, -bearing, 0);

            yield return new WaitForSeconds(SlowUpdatePeriod * 1000.0f - (Time.time - startTime));
        }
    }

    void FixedUpdate() {
        //transform.rotation = Quaternion.Euler(-gyro.attitude.eulerAngles.x, gyro.attitude.eulerAngles.y, -gyro.attitude.eulerAngles.z);
        Debug.Log(locationService.GetCurrentHeading());
        //transform.rotation = Quaternion.Euler(0, -locationService.GetCurrentHeading(), 0);
    }

    public void SetTarget(float latitude, float longitude) {
        target.latitude = latitude;
        target.longitude = longitude;
    }
}
