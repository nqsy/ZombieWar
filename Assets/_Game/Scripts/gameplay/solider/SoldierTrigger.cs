using UnityEngine;

public class SoldierTrigger : MonoBehaviour
{
    //for anim trigger
    public void OnTriggerEndDeath()
    {
        UIGameplay.instance.OpenLosePopup();
    }
}
