using UnityEngine;

public class CameraController : MonoBehaviour {

    public bool zoomedIn;
    public float zoomSensitivity;
    public float touchZoomSensitivity;

    public float minCameraZoom;
    public float maxCameraZoom;

    [SerializeField] MapController currentApartment;
    [SerializeField] MenuController menuController;

    public float touchPanSensitivity;
    public Vector3 mouseTrackingOrigin;
    public Vector3 touchTrackingOrigin;
    public RectTransform dragTouchRect;

    // Edges for the camera to stop panning at
    // TODO: add these to the map controller and get them from there at start
    float leftEdgeZoomed;
    float rightEdgeZoomed;
    float leftEdgeNorm;
    float rightEdgeNorm;

    public bool isMousePanning;
    

    private void Start()
    {
        zoomedIn = true;
        RefreshCamPanLimits();
        Camera mainCam = Camera.main;
        mainCam.transform.position = new Vector3(0, mainCam.transform.position.y, mainCam.transform.position.z);
    }

    public void SwitchZoomLevels()
    {
        Camera mainCam = Camera.main;
        if (zoomedIn)
        {
            // Zoom out
            mainCam.orthographicSize = 3.22f;
            mainCam.transform.position = new Vector3(0, mainCam.transform.position.y, mainCam.transform.position.z);
            zoomedIn = false;
        }
        else
        {
            // Zoom in
            mainCam.orthographicSize = 1.08f;
            mainCam.transform.position = new Vector3(0, mainCam.transform.position.y, mainCam.transform.position.z);
            zoomedIn = true;
        }
    }

    public void RefreshForNewApt()
    {
        currentApartment = FindObjectOfType<MapController>();
        RefreshCamPanLimits();
    }

    void RefreshCamPanLimits()
    {

        leftEdgeZoomed = currentApartment.leftPanValZoomed;
        rightEdgeZoomed = currentApartment.rightPanValZoomed;
        leftEdgeNorm = currentApartment.leftmostPanValue;
        rightEdgeNorm = currentApartment.rightmostPanValue;
    }

    void HandleCamPanning()
    {
        float camZoom = Camera.main.orthographicSize;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minCameraZoom, maxCameraZoom);

        // Handle touch dragging for SIDE TO SIDE movement
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && RectTransformUtility.RectangleContainsScreenPoint(dragTouchRect, Input.GetTouch(0).position))
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            transform.Translate(-touchDeltaPosition.x * touchPanSensitivity * Time.deltaTime, 0, 0);

            if (zoomedIn)
            {
                transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftEdgeZoomed, rightEdgeZoomed),
               Mathf.Clamp(transform.position.y, transform.position.y, transform.position.y), 0);
            }
            else
            {
                transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftEdgeNorm, rightEdgeNorm),
               Mathf.Clamp(transform.position.y, transform.position.y, transform.position.y), 0);
            }

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
            Vector2 moveVector = new Vector2(-currentCamPos.x / 2 * touchPanSensitivity * Time.smoothDeltaTime, 0);

            transform.Translate(moveVector, Space.Self);

            if (zoomedIn)
            {
                transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftEdgeZoomed, rightEdgeZoomed),
                                transform.position.y, 0);
            }
            else
            {
                transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftEdgeNorm, rightEdgeNorm),
                                transform.position.y, 0);
            }

        }

        if (Input.GetMouseButtonUp(1))
        {
            isMousePanning = false;
        }

    
}

    // Update is called once per frame
    void LateUpdate() {
        if (!menuController.isMenuOpen)
        {
            HandleCamPanning();
        }
    }
}
