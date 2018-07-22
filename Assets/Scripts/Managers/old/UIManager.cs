using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour {

    static UIManager uiManager;
    public static UIManager instance
    {
        get
        {
            if (!uiManager)
            {
                uiManager = FindObjectOfType(typeof(UIManager)) as UIManager;

                if (!uiManager)
                {
                    Debug.LogError("There needs to be one active DataManager script on a GameObject in my scene.");
                }
            }

            return uiManager;
        }
    }

    public GameObject ShopWindow;
    public GameObject TowerDetailsWindow;
    public GameObject WelcomeWindow;
    public GameObject DebugWindow;

    public bool IsTowerSelected { get; set; }

    public void Init()
    {
        HideAll();
        IsTowerSelected = false;
        StartCoroutine(ShowWelcomeWindow(3.0f));
    }

    public void HideAll()
    {
        ShopWindow.SetActive(false);
        TowerDetailsWindow.SetActive(false);
        WelcomeWindow.SetActive(false);
        DebugWindow.SetActive(false);
    }

    public void ShowShop(bool x)
    {
        HideAll();
        ShopWindow.SetActive(x);
    }

    public void ShowTowerDetailsWindow(GameObject tower)
    {
        HideAll();

        if (tower != null)
        TowerDetailsWindow.SetActive(true);

        TowerDetailsWindow.GetComponent<TowerDetailsView>().UpdateTowerInfo(tower.GetComponent<TowerCode>());
    }

    public void ShowDebug(bool x)
    {
        HideAll();
        DebugWindow.SetActive(x);
    }

    IEnumerator ShowWelcomeWindow(float duration)
    {
        WelcomeWindow.SetActive(true);
        yield return new WaitForSeconds(duration);
        WelcomeWindow.SetActive(false);
    }

}
