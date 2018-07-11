using UnityEngine;
using UnityEngine.UI;

public class WelcomeWindowView : MonoBehaviour {

    public Text awayDuraionTxt;

    internal void SetAwayDudration(string v)
    {
        awayDuraionTxt.text = "You were away for: " + string.Format("{0:0.#}", v) + "s";
        //awayDuraionTxt.text = "You were away for: " + v + "s";
    }
}
