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
        // No movement if enemy is "dazed"
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
        // Amount of arrows created is based on how much health the enemy has
        for (int i = 0; i < _maxHealth; i++)
        {
            GameObject arrow = new GameObject("Arrow0" + (i + 1));             // Create the game object and name it
            arrow.transform.parent = _arrowHolder.transform;                   // Make it a child of the empty holder object
            SpriteRenderer renderer = arrow.AddComponent<SpriteRenderer>();    // Add a sprite renderer
            renderer.sprite = Resources.Load<Sprite>("Arrow");                 // Apply the sprite from the resources folder

            // Apply direction for each arrow
            int dir = ArrowDirection();

            // If "Vertical" change color and rotation
            if (dir == 0)                                                     
            {
                renderer.color = Color.magenta;
                arrow.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            // If "Horizontal" change color and rotation
            else if (dir == 1)                                                  
            {
                renderer.color = Color.cyan;
                arrow.transform.rotation = Quaternion.Euler(0, 0, 90);
            }

            // Change position of arrows based on how many there are
            //
            // ---Maybe look into a better way to do this with some sort of sorting or something like a UI Horizontal Layout Group---
            //
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

            // Add arrows to the list IN ORDER so that you have to attack in that order
            _arrowList.Add(arrow);
        }

    }

    // Shitty translate movement, will be changed
    protected virtual void Move()
    {
        transform.Translate(Vector2.left * _speed * Time.deltaTime);
    }


    // Really need to look into a better way to test if the arrow direction matches the attack direction
    // Testing color works for now, but it's kinda ugly
    public virtual void TakeDamage(int direction)
    {
        // If attack direction is vertical AND the first arrow in the list is vertical
        if (direction == 0 && _arrowList[0].GetComponent<SpriteRenderer>().color == Color.magenta)
        {
            DamageFeedback();

            // Vertical Death
            if (_currentHealth <= 0)
            {
                Death(0);
            }
        }

        // If attack direction is horizontal AND the first arrow in the list is horizontal
        else if (direction == 1 && _arrowList[0].GetComponent<SpriteRenderer>().color == Color.cyan)
        {
            DamageFeedback();

            // Horizontal Death
            if (_currentHealth <= 0)
            {
                Death(1);
            }
        }

        // FOR LATER -- For when the player hits an enemy with the wrong attack direction

/*      else if (direction == 0 && _arrowList[0].GetComponent<SpriteRenderer>().color != Color.magenta)
        {

        }
        else if (direction == 1 && _arrowList[0].GetComponent<SpriteRenderer>().color != Color.cyan)
        {

        }
*/

    }

    private void DamageFeedback()
    {
        // Dazed timer, knockback and decrease health
        _dazedTime = _startDazedTime;
        _rb.AddForce(new Vector2(_knockbackForce, 0));
        _currentHealth -= 1;

        // Remove arrow from list and destroy the arrow object
        _arrowList.RemoveAt(0);
        Destroy(_arrowHolder.transform.GetChild(0).gameObject);
        // PUT METHOD TO REORGANIZE ARROW POSITION HERE 
    }

    // Create enemy halves and candy particles, then destroy
    public void Death(int direction)
    {
        if (direction == 0)
        {
            GameObject verticalHalves = Instantiate(_vertical, transform.position, transform.rotation);
        }
        else if (direction == 1)
        {
            GameObject horizontalHalves = Instantiate(_horizontal, transform.position, transform.rotation);
        }
        ParticleSystem candy = Instantiate(_candyParticle, transform.position + new Vector3(0, 0, -.05f), _candyParticle.transform.rotation);
        Destroy(gameObject);
    }


    // Base class is random between vertical and horizontal. Can be overwritten for specific enemies
    public virtual int ArrowDirection()
    {
        return Random.Range(0, 2);
    }
}