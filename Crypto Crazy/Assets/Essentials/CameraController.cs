using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public bool zoomedIn;
    public float zoomSensitivity;
    public float touchZoomSensitivity;

    public float minCameraZoom;
    public float maxCameraZoom;

    public MapController currentApartment;

   
    public float touchPanSensitivity;
    public Vector3 mouseTrackingOrigin;
    public Vector3 touchTrackingOrigin;
    public RectTransform dragTouchRect;

    public bool isMousePanning;
    

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
        SwitchZoomLevels();
    }

    public void RefreshForNewApt()
    {
        currentApartment = FindObjectOfType<MapController>();
    }
	
	// Update is called once per frame
	void LateUpdate () {

        float camZoom = Camera.main.orthographicSize;


       /* camZoom += zoomSensitivity * -Input.GetAxis("Mouse ScrollWheel");
        camZoom = Mathf.Clamp(camZoom, minCameraZoom, maxCameraZoom);

        Camera.main.orthographicSize = camZoom;*/

        // Zoom behaviour
        /*
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


        }*/


        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minCameraZoom, maxCameraZoom);


        // Handle touch dragging for SIDE TO SIDE movement
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && RectTransformUtility.RectangleContainsScreenPoint(dragTouchRect, Input.GetTouch(0).position))
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            transform.Translate(-touchDeltaPosition.x * touchPanSensitivity * Time.deltaTime, 0, 0);


            transform.position = new Vector3(Mathf.Clamp(transform.position.x, currentApartment.leftmostScrollValue, currentApartment.rightmostScrollValue),
               Mathf.Clamp(transform.position.y, transform.position.y, transform.position.y), 0);
        }



        // Mouse panning solution (WIP) TODO: make this a bit nicer
        if (Input.GetMouseButtonDown(1) && RectTransformUtility.RectangleContainsScreenPoint(dragTouchRect, Input.mousePosition))
        {
            mouseTrackingOrigin = Input.mousePosition;
            isMousePanning = true;
        }


        if (isMousePanning)
        {
            Vector2 currentCamPos = (Input.mousePosition - mouseTrackingOrigin);


            Vector2 moveVector = new Vector2(-currentCamPos.x * touchPanSensitivity * Time.deltaTime, 0);

            transform.Translate(moveVector, Space.Self);



            transform.position = new Vector3(Mathf.Clamp(transform.position.x, currentApartment.leftmostScrollValue, currentApartment.rightmostScrollValue), 
                transform.position.y, 0);
            
        }


        if (Input.GetMouseButtonUp(1))
        {
            isMousePanning = false;
        }
        
    }
}
