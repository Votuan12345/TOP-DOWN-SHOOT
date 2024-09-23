using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collection : MonoBehaviour
{
    public GameObject weapon;

    private void OnTriggerEnter2D(Collider2D col)
    {
        Player player = col.gameObject.GetComponent<Player>();
        if(player != null)
        {
            player.CreateWeapon(weapon);
            Destroy(gameObject);
        }
    }
}
