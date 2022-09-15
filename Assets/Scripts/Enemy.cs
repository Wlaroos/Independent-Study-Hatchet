using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int[] _health;
    private float _dazedTime;
    [SerializeField] private float _startDazedTime;
    [SerializeField] GameObject _horizontal;
    [SerializeField] GameObject _vertical;
    [SerializeField] ParticleSystem _candyParticle;

    private Animator Anim;

    private void Start()
    {
        Anim = GetComponent<Animator>();
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
            GameObject verticalHalves = Instantiate(_vertical, transform.position, transform.rotation);
            Debug.Log("Vertical");
            Destroy(gameObject);
        }
        else if (direction == 1)
        {
            _dazedTime = _startDazedTime;
            GameObject horizontalHalves = Instantiate(_horizontal, transform.position, transform.rotation);
            Debug.Log("Horizontal");
            Destroy(gameObject);
        }

        ParticleSystem candy = Instantiate(_candyParticle, transform.position + new Vector3(0, 0, -.05f), _candyParticle.transform.rotation);
    }
}
