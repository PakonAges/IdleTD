using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CreepMain : MonoBehaviour {

    public int MaxHP { get; set; }
    public int CoinsReward { get; set; }
    int currentHP;

    //public Image healthBar;

    public void Init(int HP, int Coins)
    {
        MaxHP = HP;
        CoinsReward = Coins;

        currentHP = MaxHP;
        //healthBar.fillAmount = currentHP;

        gameObject.GetComponent<NavMeshAgent>().enabled = true;
    }

    public bool RecieveDamageAndDie(int dmg)
    {
        currentHP -= dmg;
        //healthBar.fillAmount = (float)currentHP / MaxHP; //fill is from 0 - 1 only

        if (!gameObject.activeInHierarchy)
        {
            return true;
        }

        if (currentHP <= 0)
        {
            Die();
            return true;
        } else
            return false;
    }

    void Die()
    {
        gameObject.SetActive(false);
        GetComponent<CreepMovement>().ResetMovement();

        //PlayerStats.instance.Coins += CoinsReward;
        //OnCreepDied();
    }

    protected virtual void OnCreepDied()
    {
        EventManager.Broadcast(gameEvent.CreepDied, new eventArgExtend() { creep = this });
    }
}