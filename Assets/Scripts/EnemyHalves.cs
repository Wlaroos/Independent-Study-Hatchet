using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHalves : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] float xForce;
    [SerializeField] float yForce;
    [SerializeField] float randomForceAmount = 200;
    [SerializeField] float randomTorqueAmount = 100;
    [SerializeField] float destroyDelay = 2.5f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        // Add random Torque and Force to lauch the body parts -- Blood particles are attached to the halves
        _rb.AddTorque(Random.Range(-randomTorqueAmount, randomTorqueAmount));
        _rb.AddForce( new Vector2(Random.Range(xForce - randomForceAmount, xForce + randomForceAmount), Random.Range(yForce - randomForceAmount, yForce + randomForceAmount)));


        // Maybe end up changing this to a coroutine that lerps opacity or size to make it "fade" away
        Invoke(nameof(Delt), destroyDelay);
    }

    private void Delt()
    {
        Destroy(this.gameObject);
    }
}
