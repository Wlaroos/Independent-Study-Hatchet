using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : MonoBehaviour
{

    public int _attackDir;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<EnemyBase>() != null)
        {
            collision.gameObject.GetComponent<EnemyBase>().TakeDamage(_attackDir);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyBase>() != null)
        {
            collision.gameObject.GetComponent<EnemyBase>().TakeDamage(_attackDir);
        }
    }
}
