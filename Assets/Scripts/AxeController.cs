using UnityEngine;

public class AxeController : MonoBehaviour
{

    // _attackDir gets set by the AttackAnimationEvent script as the swing starts
    public int _attackDir;

    // Calls the TakeDamage method on the enemy base class
    // Need to fix this being called multiple times -- Maybe just add a very small delay after a successful call
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<EnemyBase>() != null)
        {
            collision.gameObject.GetComponent<EnemyBase>().TakeDamage(_attackDir);
        }
        if (collision.gameObject.GetComponent<WitchOrb>() != null)
        {
            collision.gameObject.GetComponent<WitchOrb>().TakeDamage(_attackDir);
        }
    }

}
