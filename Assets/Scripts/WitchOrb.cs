using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WitchOrb : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] ParticleSystem _impactParticles;
    [SerializeField] float _rotationSpeed;
    [SerializeField] int _damage;
    Vector3 _direction;

    PlayerController _playerRef;
    Rigidbody2D _rb;

    [SerializeField] private Rigidbody2D _artRB;
    private GameObject _arrowHolder;
    private List<GameObject> _arrowList = new List<GameObject>();

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerRef = FindObjectOfType<PlayerController>();
        _arrowHolder = transform.Find("ArrowHolder").gameObject;
    }

    private void Start()
    {
        AddArrows();
    }

    private void FixedUpdate()
    {
        _direction = _playerRef.transform.position - transform.position;

        if (_direction.x - _direction.x > 0)
        {
            _speed = -_speed;
            _rotationSpeed = -_rotationSpeed;
        }

        _rb.velocity = new Vector2(_speed * _direction.normalized.x, _speed * _direction.normalized.y);
        _artRB.velocity = new Vector2(_speed * _direction.normalized.x, _speed * _direction.normalized.y);
        _artRB.angularVelocity = _rotationSpeed;
    }

    void LateUpdate()
    {
        _arrowHolder.transform.rotation = Quaternion.Euler(0.0f, 0.0f, gameObject.transform.rotation.z * -1.0f);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        PlayerController playerRef = collision.gameObject.transform.GetComponent<PlayerController>();

        if (playerRef != null)
        {
            int dir;
            if (transform.forward.x == 1) dir = 1; else dir = -1;

            if (_impactParticles != null) Instantiate(_impactParticles,transform);
            playerRef.DecreaseHealth(_damage, dir);
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(int direction)
    {
        // If attack direction is vertical AND the first arrow in the list is vertical
        if (direction == 0 && _arrowList[0].GetComponent<SpriteRenderer>().color == Color.magenta)
        {
            if (_impactParticles != null) Instantiate(_impactParticles, transform);
            Destroy(this.gameObject);
        }

        // If attack direction is horizontal AND the first arrow in the list is horizontal
        else if (direction == 1 && _arrowList[0].GetComponent<SpriteRenderer>().color == Color.cyan)
        {
            if (_impactParticles != null) Instantiate(_impactParticles, transform);
            Destroy(this.gameObject);
        }

        // FOR LATER -- For when the player hits an enemy with the wrong attack direction
        else if (direction == 0 && _arrowList[0].GetComponent<SpriteRenderer>().color != Color.magenta)
        {

        }
        else if (direction == 1 && _arrowList[0].GetComponent<SpriteRenderer>().color != Color.cyan)
        {

        }
    }

    private void AddArrows()
    {
        // Amount of arrows created is based on how much health the enemy has
            GameObject arrow = new GameObject("Arrow0" + 1);             // Create the game object and name it
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

    private int ArrowDirection()
    {
        return Random.Range(0, 2);
    }

}