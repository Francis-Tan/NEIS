using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gbullet_pooler : MonoBehaviour
{
    public Gunner_Bullet gbullet;
    private List<Gunner_Bullet> bullets = new List<Gunner_Bullet>();
    private void Start()
    {
        if (gbullet == null)
        {
            Debug.LogError("Gunner bullet prefab is empty");
        }
        gbullet.gameObject.SetActive(false);
    }

    private void SpawnBullets()
    {
        for (int i = 0; i < 10; ++i)
        {
            bullets.Add(Instantiate(gbullet));
        }
    }

    private Gunner_Bullet GetInactiveBullet()
    {
        for (int i = 0; i < bullets.Count; ++i)
        {
            if (!bullets[i].gameObject.activeSelf)
            {
                return bullets[i];
            }
        }
        SpawnBullets();
        return GetInactiveBullet();
    }

    public void FireBullet()
    {
        Gunner_Bullet bullet = GetInactiveBullet();
        bullet.reset(transform.position, transform.rotation);
        bullet.gameObject.SetActive(true);
    }

    public void Die() {
        for (int i = 0; i < bullets.Count; ++i)
        {
            Destroy(bullets[i].gameObject);
        }
        Destroy(gameObject);
    }
}
