using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
    float delayTime = 1f;
    float currentTime = 0f;

    private void Update()
    {
        currentTime += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerRef = collision.gameObject.transform.GetComponent<PlayerController>();

        if (playerRef != null)
        {
            SendMessageUpwards("Attack");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        CandyCrate crateRef = collision.gameObject.transform.GetComponent<CandyCrate>();

        if (crateRef != null && transform.parent.GetComponent<EnemyBase>().GetAttacker() != null && currentTime > delayTime)
        {
            SendMessageUpwards("Attack");
            currentTime = 0;
        }
    }

}
