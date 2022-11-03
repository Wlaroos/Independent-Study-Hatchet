using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerRef = collision.gameObject.transform.GetComponent<PlayerController>();

        if (playerRef != null)
        {
            SendMessageUpwards("Attack");
        }
    }

}
