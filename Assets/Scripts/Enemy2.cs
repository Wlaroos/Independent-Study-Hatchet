using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{

    [SerializeField] private float _speed;
    private int _maxHealth = 2;
    private int _currentHealth;
    private float _dazedTime;
    [SerializeField] private float _startDazedTime;
    [SerializeField] GameObject _horizontal;
    [SerializeField] GameObject _vertical;
    [SerializeField] ParticleSystem _candyParticle;

    private Animator Anim;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        Anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_dazedTime <= 0)
        {
            _speed = .5f;
        }
        else
        {
            _speed = 0;
            _dazedTime -= Time.deltaTime;
        }


        transform.Translate(Vector2.left * _speed * Time.deltaTime);
    }

    public void TakeDamage(int direction)
    {
        if (direction == 0)
        {
            _dazedTime = _startDazedTime;
            _rb.AddForce(new Vector2(150, 0));
            _currentHealth -= 1;
            if (_currentHealth <= 0)
            {
                Kill(0);
            }
        }
        else if (direction == 1)
        {
            _dazedTime = _startDazedTime;
            _rb.AddForce(new Vector2(150, 0));
            _currentHealth -= 1;
            if (_currentHealth <= 0)
            {
                Kill(1);
            }
        }

    }

    public void Kill(int direction)
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

}
