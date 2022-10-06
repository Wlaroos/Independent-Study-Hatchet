using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float _maxSpeed = 5f;
    private float _speed;
    [SerializeField] protected int _maxHealth = 1;
    private int _currentHealth;
    [SerializeField] protected float _startDazedTime;
    private float _dazedTime;
    [SerializeField] GameObject _horizontal;
    [SerializeField] GameObject _vertical;
    [SerializeField] ParticleSystem _candyParticle;

    private Animator Anim;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        _speed = _maxSpeed;
        Anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_dazedTime <= 0)
        {
            Move();
            _speed = _maxSpeed;
        }
        else
        {
            _speed = 0;
            _dazedTime -= Time.deltaTime;
        }
    }

    protected virtual void Move()
    {
        transform.Translate(Vector2.left * _speed * Time.deltaTime);
    }

    public virtual void TakeDamage(int direction)
    {
        if (direction == 0)
        {
            _dazedTime = _startDazedTime;
            _rb.AddForce(new Vector2(150, 0));
            _currentHealth -= 1;
            if (_currentHealth <= 0)
            {
                Death(0);
            }
        }
        else if (direction == 1)
        {
            _dazedTime = _startDazedTime;
            _rb.AddForce(new Vector2(150, 0));
            _currentHealth -= 1;
            if (_currentHealth <= 0)
            {
                Death(1);
            }
        }
    }

    public void Death(int direction)
    {
        if (direction == 0)
        {
            GameObject verticalHalves = Instantiate(_vertical, transform.position, transform.rotation);
            Debug.Log("Vertical");

        }
        else if (direction == 1)
        {
            GameObject horizontalHalves = Instantiate(_horizontal, transform.position, transform.rotation);
            Debug.Log("Horizontal");
        }
        Destroy(gameObject);
        ParticleSystem candy = Instantiate(_candyParticle, transform.position + new Vector3(0, 0, -.05f), _candyParticle.transform.rotation);
    }

    public virtual void TestMethod()
    {
        Debug.Log("EnemyBase TEST");
    }
}