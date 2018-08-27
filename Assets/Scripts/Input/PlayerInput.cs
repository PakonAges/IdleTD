using UnityEngine;
using DigitalRubyShared;
using Cinemachine;

public class PlayerInput : MonoBehaviour
{
    [Header("Camera Setup")]
    public Transform CameraFocus;
    public CinemachineVirtualCamera MainVirtualCamera;
    public Camera MainCam;
    public Vector3 DefaultCameraFocus = new Vector3(6.0f, 0.0f, -5.0f);
    public int defaultZoom = 20;
    public int maxZoom = 30;
    public int minZoom = 15;

    [Header("Input")]
    public SelectionData TappedObject;
    public LayerMask ClickSelectionLayer;

    private Vector3 _dragOffset;
    private Vector3 _origFocusPos;

    //public CinemachineVirtualCamera VirtualCamera;

    private PanGestureRecognizer panGesture;
    private TapGestureRecognizer tapGesture;
    private ScaleGestureRecognizer scaleGesture;

    public bool ShowToches = false;

    void Start () {
        CreateTapGesture();
        CreatePanGesture();
        CreateScaleGesture();

        panGesture.AllowSimultaneousExecution(scaleGesture);

        // prevent the one special no-pass button from passing through,
        //  even though the parent scroll view allows pass through (see FingerScript.PassThroughObjects)
        FingersScript.Instance.CaptureGestureHandler = CaptureGestureHandler;

        // show touches, only do this for debugging as it can interfere with other canvases
        FingersScript.Instance.ShowTouches = ShowToches;

        SetupDefaultCamera();
    }

    void SetupDefaultCamera()
    {
        CameraFocus.position = DefaultCameraFocus;
        MainVirtualCamera.m_Lens.FieldOfView = defaultZoom;
    }

    private void CreateTapGesture()
    {
        tapGesture = new TapGestureRecognizer();
        tapGesture.StateUpdated += TapGestureCallback;
        FingersScript.Instance.AddGesture(tapGesture);
    }

    private void TapGestureCallback(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Ended)
        {
            //DebugText("Tapped at {0}, {1}", gesture.FocusX, gesture.FocusY);
            var selectedTile = MouseClickToMapTile(gesture.FocusX, gesture.FocusY);

            if (selectedTile != null)
            {
                TappedObject.TileSelected(selectedTile);
            }
        }
    }

    private void CreatePanGesture()
    {
        panGesture = new PanGestureRecognizer();
        panGesture.MinimumNumberOfTouchesToTrack = 1;
        panGesture.StateUpdated += PanGestureCallback;
        FingersScript.Instance.AddGesture(panGesture);
    }

    private void PanGestureCallback(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Began)
        {
            //DebugText("Drag began: {0}, {1}", gesture.FocusX, gesture.FocusY);
            BeginDrag(gesture.FocusX, gesture.FocusY);
        }
        else if (gesture.State == GestureRecognizerState.Executing)
        {
            //DebugText("Drag moved: {0}, {1}", gesture.FocusX, gesture.FocusY);
            DragTo(gesture.FocusX, gesture.FocusY);
        }
        else if (gesture.State == GestureRecognizerState.Ended)
        {
            //DebugText("Drag end: {0}, {1}, delta: {2}, {3}", gesture.FocusX, gesture.FocusY, gesture.DeltaX, gesture.DeltaY);
            EndDrag(gesture.FocusX, gesture.FocusY);
        }
    }

    void BeginDrag(float screenX, float screenY)
    {
        Vector3 touchScreenPosition = new Vector3(screenX, screenY, 0.0f);
        Ray touchRay = MainCam.ScreenPointToRay(touchScreenPosition);
        float cameraHeightPosition = touchRay.origin.y;
        Vector3 DirectionNormal = touchRay.direction / touchRay.direction.y;

        _dragOffset = touchRay.origin - DirectionNormal * cameraHeightPosition;
        _origFocusPos = CameraFocus.position;
    }

    void DragTo(float screenX, float screenY)
    {
        Vector3 pos = new Vector3(screenX, screenY, 0.0f);
        Ray ray = MainCam.ScreenPointToRay(pos);

        float delta = ray.origin.y;
        Vector3 dirNorm = ray.direction / ray.direction.y;
        Vector3 intersetionPos = ray.origin - dirNorm * delta;

        CameraFocus.position = _origFocusPos - (intersetionPos - _dragOffset);
    }

    void EndDrag(float screenX, float screenY)
    {
        _dragOffset = Vector3.zero;
    }

    private void CreateScaleGesture()
    {
        scaleGesture = new ScaleGestureRecognizer();
        scaleGesture.StateUpdated += ScaleGestureCallback;
        FingersScript.Instance.AddGesture(scaleGesture);
    }

    private void ScaleGestureCallback(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Executing)
        {
            //DebugText("Scaled: {0}, Focus: {1}, {2}", scaleGesture.ScaleMultiplier, scaleGesture.FocusX, scaleGesture.FocusY);
            MainVirtualCamera.m_Lens.FieldOfView = Mathf.Clamp(MainVirtualCamera.m_Lens.FieldOfView * scaleGesture.ScaleMultiplier,minZoom,maxZoom);
            
        }
    }

    private static bool? CaptureGestureHandler(GameObject obj)
    {
        // I've named objects PassThrough* if the gesture should pass through and NoPass* if the gesture should be gobbled up, everything else gets default behavior
        if (obj.name.StartsWith("PassThrough"))
        {
            // allow the pass through for any element named "PassThrough*"
            return false;
        }
        else if (obj.name.StartsWith("NoPass"))
        {
            // prevent the gesture from passing through, this is done on some of the buttons and the bottom text so that only
            // the triple tap gesture can tap on it
            return true;
        }

        // fall-back to default behavior for anything else
        return null;
    }

    private void DebugText(string text, params object[] format)
    {
        //bottomLabel.text = string.Format(text, format);
        Debug.Log(string.Format(text, format));
    }

    private void LateUpdate()
    {
        int touchCount = Input.touchCount;
        if (FingersScript.Instance.TreatMousePointerAsFinger && Input.mousePresent)
        {
            touchCount += (Input.GetMouseButton(0) ? 1 : 0);
            touchCount += (Input.GetMouseButton(1) ? 1 : 0);
            touchCount += (Input.GetMouseButton(2) ? 1 : 0);
        }
        string touchIds = string.Empty;
        int gestureTouchCount = 0;
        foreach (GestureRecognizer g in FingersScript.Instance.Gestures)
        {
            gestureTouchCount += g.CurrentTrackedTouches.Count;
        }
        foreach (GestureTouch t in FingersScript.Instance.CurrentTouches)
        {
            touchIds += ":" + t.Id + ":";
        }
    }

    Vector3 MouseToGoundPlane(float screenX, float screenY)
    {
        Vector3 mousePos = new Vector3(screenX, screenY, 0.0f);
        Ray mouseRay = MainCam.ScreenPointToRay(mousePos);
        if (mouseRay.direction.y >= 0)
        {
            Debug.LogError("Mouse is pointing Up?");
            return Vector3.zero;
        }
        float rayLength = (mouseRay.origin.y / mouseRay.direction.y);
        return mouseRay.origin - (mouseRay.direction * rayLength);
    }

    MapTile MouseClickToMapTile(float screenX, float screenY)
    {
        Vector3 mousePos = new Vector3(screenX, screenY, 0.0f);
        Ray mouseRay = MainCam.ScreenPointToRay(mousePos);
        RaycastHit hitInfo;

        if (Physics.Raycast(mouseRay, out hitInfo, ClickSelectionLayer))
        {
            var ObjectHitted = hitInfo.rigidbody.gameObject;

            if (ObjectHitted == null)
            {
                return null;
            }

            var Tile = hitInfo.rigidbody.gameObject.GetComponent<MapTile>();

            if (Tile == null)
            {
                Debug.Log(string.Format("Clicked on the tile: {0}, but no MapTile script attached", ObjectHitted.name.ToString()));
            }

            return Tile;
        }
        else
        {
            return null;
        }
    }
}
