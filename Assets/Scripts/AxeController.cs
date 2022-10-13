using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : MonoBehaviour
{

    public int _attackDir;

    // Direction gets set in the AttackAnimationEvent script then calls damage on the enemy base class
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<EnemyBase>() != null)
        {
            collision.gameObject.GetComponent<EnemyBase>().TakeDamage(_attackDir);
        }
    }

}
