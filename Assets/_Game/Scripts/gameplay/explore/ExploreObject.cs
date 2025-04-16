using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreObject : MonoBehaviour
{
    Cooldown cdEffect;

    public void OnSpawn()
    {
        cdEffect = new Cooldown(0.2f);
    }

    private void Update()
    {
        if(cdEffect.IsFinishing)
        {
            Despawn();
        }

        cdEffect.ReduceCooldown();
    }

    public void Despawn()
    {
        ExploreManager.instance.DespawnExplore(gameObject);
    }    
}
