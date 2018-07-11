using UnityEngine;

public class DebugButtonUI : MonoBehaviour {

    bool debugActive = false;

    public void ShowDebug()
    {
        UIManager.instance.ShowDebug(!debugActive);
        debugActive = !debugActive;
    }
}
