using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public bool zoomedIn;
    public float zoomSensitivity;
    public float touchZoomSensitivity;

    public float minCameraZoom;
    public float maxCameraZoom;

    public void SwitchZoomLevels()
    {
        if (zoomedIn)
        {
            Camera.main.orthographicSize = 3.22f;
            zoomedIn = false;
        }
        else
        {
            Camera.main.orthographicSize = 1.08f;
            zoomedIn = true;
        }
    }

	// Use this for initialization
	void Start () {
        zoomedIn = true;

    }
	
	// Update is called once per frame
	void Update () {

        float camZoom = Camera.main.orthographicSize;


       /* camZoom += zoomSensitivity * -Input.GetAxis("Mouse ScrollWheel");
        camZoom = Mathf.Clamp(camZoom, minCameraZoom, maxCameraZoom);

        Camera.main.orthographicSize = camZoom;*/


        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 zeroPrevePos = touchZero.position - touchZero.deltaPosition;
            Vector2 onePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevFrameTouchDist = (zeroPrevePos - onePrevPos).magnitude;
            float curFrameTouchDist = (touchZero.position - touchOne.position).magnitude;

            float differenceInDistances = prevFrameTouchDist - curFrameTouchDist;

            Camera.main.orthographicSize += differenceInDistances * zoomSensitivity * Time.deltaTime;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minCameraZoom, maxCameraZoom);


        }


        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minCameraZoom, maxCameraZoom);

    }
}
