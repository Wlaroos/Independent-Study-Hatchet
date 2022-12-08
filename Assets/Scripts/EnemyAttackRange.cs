using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
    // Delay time for crate attacks
    float delayTime = 2f;
    float currentTime = 0f;

    private void Update()
    {
        currentTime += Time.deltaTime;
    }

    // Attack player if too close
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerRef = collision.gameObject.transform.GetComponent<PlayerController>();

        if (playerRef != null)
        {
            SendMessageUpwards("Attack");
        }
    }

    // Attack the crate if close enough
    private void OnTriggerStay2D(Collider2D collision)
    {
        CandyCrate crateRef = collision.gameObject.transform.GetComponent<CandyCrate>();

        // Checks to see if this enemy is the one allowed to attack the crate
        if (crateRef != null && transform.parent.GetComponent<EnemyBase>().GetAttacker() == transform.parent.GetComponent<EnemyBase>() && currentTime > delayTime)
        {
            SendMessageUpwards("Attack");
            currentTime = 0;
        }
    }

}
