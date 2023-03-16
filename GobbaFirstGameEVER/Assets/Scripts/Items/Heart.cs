using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    [SerializeField] int heal = 5;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerCombat player = collision.GetComponent<PlayerCombat>();
        if (player)
        {
            AudioManager.Play("PickUp");
            player.Health += ((player.MaxHealth * heal) / 100);
            Destroy(gameObject);
        }
    }
}
