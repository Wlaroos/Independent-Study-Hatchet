using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHalves : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] float xForce;
    [SerializeField] float yForce;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        _rb.AddTorque(UnityEngine.Random.Range(-100, 100));

        _rb.AddForce( new Vector2(UnityEngine.Random.Range(xForce - 200, xForce + 200), UnityEngine.Random.Range(yForce - 200, yForce + 200)));

        Invoke(nameof(Delt), 2.5f);
    }

    private void Delt()
    {
        Destroy(this.gameObject);
    }
}
