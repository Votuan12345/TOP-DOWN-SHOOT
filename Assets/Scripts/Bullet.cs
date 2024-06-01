using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public bool isSound;

    private enum Target
    {
        Player,
        Enemy
    }

    [SerializeField] private Target target;

    private void Start()
    {
        if (isSound == false) return;
        if(AudioController.instance != null)
        {
            AudioController.instance.PlaySound(AudioController.instance.gunSound);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(target == Target.Player)
        {
            Player player = col.GetComponent<Player>();
            if(player != null)
            {
                player.ChangeHealth(damage * -1);
                Destroy(gameObject);
            }
        }
        else if(target == Target.Enemy)
        {
            EnemyAI enemy = col.GetComponent<EnemyAI>();
            if(enemy != null)
            {
                enemy.Hit(damage);
                Destroy(gameObject);
            }
        }

        if (col.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
        
    }
}
