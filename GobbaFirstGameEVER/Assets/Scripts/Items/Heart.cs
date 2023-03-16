using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    [SerializeField] float heal = 5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerCombat player = collision.GetComponent<PlayerCombat>();
        if (player)
        {
            AudioManager.Play("PickUp");
            player.Health += (int)Mathf.Round(player.MaxHealth * heal / 100f);
            Destroy(gameObject);
        }
    }
}
