using UnityEngine;

public class HudUI : MonoBehaviour {

    public GameObject BotHUD;

    public void HideBotHud(bool x)
    {
        BotHUD.SetActive(x);
    }

}
