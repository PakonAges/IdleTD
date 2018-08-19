using UnityEngine;
using DigitalRubyShared;

public class PlayerInput : MonoBehaviour
{
    public Transform deBugBeginDrag;
    private Vector3 _dragOffset;
    private Vector3 _origFocusPos;

    public Transform CameraFocus;
    public Camera MainCam;
    private Vector3 StartDragPoint;

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
            DebugText("Tapped at {0}, {1}", gesture.FocusX, gesture.FocusY);

            //Vector3 pos = new Vector3(gesture.FocusX, gesture.FocusY, 0.0f);
            //Ray ray = MainCam.ScreenPointToRay(pos);
            //float delta = ray.origin.y;
            //Vector3 dirNorm = ray.direction / ray.direction.y;
            //Vector3 intersetionPos = ray.origin - dirNorm * delta;

            //deBugBeginDrag.position = intersetionPos;
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
            DebugText("Drag began: {0}, {1}", gesture.FocusX, gesture.FocusY);
            BeginDrag(gesture.FocusX, gesture.FocusY);
        }
        else if (gesture.State == GestureRecognizerState.Executing)
        {
            DebugText("Drag moved: {0}, {1}", gesture.FocusX, gesture.FocusY);
            DragTo(gesture.FocusX, gesture.FocusY);
        }
        else if (gesture.State == GestureRecognizerState.Ended)
        {
            DebugText("Drag end: {0}, {1}, delta: {2}, {3}", gesture.FocusX, gesture.FocusY, gesture.DeltaX, gesture.DeltaY);
            EndDrag(gesture.FocusX, gesture.FocusY);
        }
    }

    void BeginDrag(float screenX, float screenY)
    {
        Vector3 pos = new Vector3(screenX, screenY, 0.0f);
        Ray ray = MainCam.ScreenPointToRay(pos);
        float delta = ray.origin.y;
        Vector3 dirNorm = ray.direction / ray.direction.y;
        _dragOffset = ray.origin - dirNorm * delta;

        _origFocusPos = CameraFocus.position;
    }

    void DragTo(float screenX, float screenY)
    {
        Vector3 pos = new Vector3(screenX, screenY, 0.0f);
        Ray ray = MainCam.ScreenPointToRay(pos);

        float delta = ray.origin.y;
        Vector3 dirNorm = ray.direction / ray.direction.y;
        Vector3 intersetionPos = ray.origin - dirNorm * delta;

        CameraFocus.position = _origFocusPos + intersetionPos - _dragOffset;
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
            DebugText("Scaled: {0}, Focus: {1}, {2}", scaleGesture.ScaleMultiplier, scaleGesture.FocusX, scaleGesture.FocusY);
            //cam.orthographicSize *= scaleGesture.ScaleMultiplier;
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
}
