using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyBase : MonoBehaviour
{

    // Movement/Health Variables
    [SerializeField] protected float _maxSpeed = 5f;
    private float _speed;
    [SerializeField] protected int _knockbackForce = 150;
    [SerializeField] protected int _maxHealth = 1;
    private int _currentHealth;

    // Dazed and iFrame variables
    [SerializeField] protected float _startDazedTime = 0.5f;
    private bool _dazed = false;
    private float numOfFlashes = 4;
    private Color flashColor = Color.red;

    // Prefab variables
    [SerializeField] GameObject _horizontal;
    [SerializeField] GameObject _vertical;
    [SerializeField] ParticleSystem _candyParticle;

    // Component/Object Variables
    private Animator _anim;
    private SpriteRenderer _sr;
    private Rigidbody2D _rb;
    private GameObject _arrowHolder;
    private List<GameObject> _arrowList = new List<GameObject>();

    private void Awake()
    {
        _currentHealth = _maxHealth;
        _speed = _maxSpeed;
        // Find a way to change color of psb file instead of sprite renderer
        //_sr = transform.Find("EnemyArt").GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _arrowHolder = transform.Find("ArrowHolder").gameObject;
        _anim = transform.Find("EnemyArt").GetComponent<Animator>();
    }

    private void Start()
    {
        // Create arrows as soon as game starts
        AddArrows();
        // Start idle animation at random location so enemies don't move in sync
        _anim.Play(0,0,Random.value);
    }

    private void Update()
    {
        // No movement if enemy is "dazed"
        if (_dazed == false)
        {
            Move();
        }

    }

    // Shitty translate movement, will be changed
    protected virtual void Move()
    {
        transform.Translate(Vector2.left * _speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<PlayerController>() != null)
        {
            int dir;
            if (transform.forward.x == 1) dir = 1; else dir = -1;

            collision.transform.GetComponent<PlayerController>().DecreaseHealth(1, dir);
        }
    }

    // Really need to look into a better way to test if the arrow direction matches the attack direction
    // Testing color works for now, but it's kinda ugly
    public virtual void TakeDamage(int direction)
    {
        // If attack direction is vertical AND the first arrow in the list is vertical
        if (direction == 0 && _arrowList[0].GetComponent<SpriteRenderer>().color == Color.magenta && _dazed == false)
        {
            DamageFeedback();

            // Vertical Death
            if (_currentHealth <= 0)
            {
                Death(0);
            }
        }

        // If attack direction is horizontal AND the first arrow in the list is horizontal
        else if (direction == 1 && _arrowList[0].GetComponent<SpriteRenderer>().color == Color.cyan && _dazed == false)
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
        // Dazed coroutine, knockback and decrease health
        StartCoroutine(IFrameCoroutine(_startDazedTime));
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

    // Duration is set to the value of _delayStartTime
    private IEnumerator IFrameCoroutine(float duration)
    {
        // Stops movement and damage from occuring
        _speed = 0; 
        _dazed = true;

        // Timer and flash setup
        float timestamp = Time.time + duration;

        //Color defaultColor = _sr.color;           // Until I fix the _sr
        Color defaultColor = Color.white;

        bool flash = false;

        // While loop for duration
        while (Time.time < timestamp)   
        {
            // If flash is true, set color to flash color, otherwise set color back to default
            //_sr.color = flash ? flashColor : defaultColor;                // Until I fix the _sr
            flash = !flash;
            yield return new WaitForSeconds(duration / numOfFlashes);
        }

        // Reset values after duration
        //_sr.color = defaultColor;                     // Until I fix the _sr
        _dazed = false;
        _speed = _maxSpeed;
    }

    private void AddArrows()
    {
        // Amount of arrows created is based on how much health the enemy has
        for (int i = 0; i < _maxHealth; i++)
        {
            GameObject arrow = new GameObject("Arrow0" + (i + 1));             // Create the game object and name it
            arrow.transform.SetParent(_arrowHolder.transform);                 // Make it a child of the empty holder object
            SpriteRenderer renderer = arrow.AddComponent<SpriteRenderer>();    // Add a sprite renderer
            arrow.AddComponent<LayoutElement>();                               // Add a layout element (Allows Sorting)
            renderer.sprite = Resources.Load<Sprite>("Arrow");                 // Apply the sprite from the resources folder'

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

            // Add arrows to the list IN ORDER so that you have to attack in that order
            _arrowList.Add(arrow);
        }

    }

    // Base class is random between vertical and horizontal. Can be overwritten for specific enemies
    public virtual int ArrowDirection()
    {
        return Random.Range(0, 2);
    }
}