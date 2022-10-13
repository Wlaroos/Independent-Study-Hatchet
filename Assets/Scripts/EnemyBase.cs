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
    [SerializeField] protected int _knockbackForce = 150;
    [SerializeField] GameObject _horizontal;
    [SerializeField] GameObject _vertical;
    [SerializeField] ParticleSystem _candyParticle;

    private Animator Anim;
    private Rigidbody2D _rb;
    private GameObject _arrowHolder;
    private List<GameObject> _arrowList = new List<GameObject>();

    private void Awake()
    {
        _currentHealth = _maxHealth;
        _speed = _maxSpeed;
        Anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _arrowHolder = transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        AddArrows();
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

    private void AddArrows()
    {
        for (int i = 0; i < _maxHealth; i++)
        {
            GameObject arrow = new GameObject("Arrow0" + (i + 1));
            arrow.transform.parent = _arrowHolder.transform;
            SpriteRenderer renderer = arrow.AddComponent<SpriteRenderer>();
            renderer.sprite = Resources.Load<Sprite>("Arrow");

            if(Random.Range(0, 2) == 0)
            {
                renderer.color = Color.magenta;
                arrow.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                renderer.color = Color.cyan;
                arrow.transform.rotation = Quaternion.Euler(0, 0, 90);
            }

            switch (_maxHealth)
            {
                case 1: arrow.transform.localPosition = new Vector3(0, 0.8f, 0); break;
                case 2:
                    {
                        switch (i + 1)
                        {
                            case 1: arrow.transform.localPosition = new Vector3(-0.25f, 0.8f, 0); break;
                            case 2: arrow.transform.localPosition = new Vector3(0.25f, 0.8f, 0); break;
                        }
                    }break;

                case 3:
                    {
                        switch (i + 1)
                        {
                            case 1: arrow.transform.localPosition = new Vector3(-0.425f, 0.8f, 0); break;
                            case 2: arrow.transform.localPosition = new Vector3(0f, 0.8f, 0); break;
                            case 3: arrow.transform.localPosition = new Vector3(0.425f, 0.8f, 0); break;
                        }
                    }break;
            }

            _arrowList.Add(arrow);
        }

    }

    protected virtual void Move()
    {
        transform.Translate(Vector2.left * _speed * Time.deltaTime);
    }

    public virtual void TakeDamage(int direction)
    {
        if (direction == 0 && _arrowList[0].GetComponent<SpriteRenderer>().color == Color.magenta)
        {
            _dazedTime = _startDazedTime;
            _rb.AddForce(new Vector2(_knockbackForce, 0));
            _currentHealth -= 1;

            _arrowList.RemoveAt(0);
            Destroy(_arrowHolder.transform.GetChild(0).gameObject);

            if (_currentHealth <= 0)
            {
                Death(0);
            }
        }
        else if (direction == 1 && _arrowList[0].GetComponent<SpriteRenderer>().color == Color.cyan)
        {
            _dazedTime = _startDazedTime;
            _rb.AddForce(new Vector2(_knockbackForce, 0));
            _currentHealth -= 1;

            _arrowList.RemoveAt(0);
            Destroy(_arrowHolder.transform.GetChild(0).gameObject);

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