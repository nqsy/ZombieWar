using UnityEngine;

public class Bomb : OtherItemObject
{
    [SerializeField] BombDetect bombDetect;

    Cooldown cdExplore;

    private void Start()
    {
        cdExplore = new Cooldown(GameConfig.instance.durationBomb);
    }

    private void Update()
    {
        if (cdExplore.IsFinishing)
        {
            Explore();
        }
        else
        {
            cdExplore.ReduceCooldown();
        }
    }

    void Explore()
    {
        ExploreManager.instance.SpawnExploreBomb(transform.position);

        foreach(var enemy in bombDetect.enemyObjects)
        {
            enemy.BeAttack(GameConfig.instance.dmgBomb);
        }

        SoundManager.instance.PlaySound(ESoundType.explore_bomb);
        Despawn();
    }
}
