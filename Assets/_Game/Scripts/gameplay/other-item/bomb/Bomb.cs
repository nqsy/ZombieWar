using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] BombDetect bombDetect;
    [SerializeField] float timeCd;
    [SerializeField] float dmg;

    Cooldown cdExplore;

    private void Start()
    {
        cdExplore = new Cooldown(timeCd);
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
            enemy.BeAttack(dmg, enemy.transform.position);
        }

        SoundManager.instance.PlaySound(ESoundType.explore_bomb);

        Destroy(gameObject);
    }
}
