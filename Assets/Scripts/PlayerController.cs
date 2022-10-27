using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class PlayerController : MonoBehaviour
{
    // Event Variables
    public event Action PlayerDamage;

    // Health Variables
    [SerializeField] int _maxHealth = 10;
    int _currentHealth;
    public int MaxHealth => _maxHealth;
    public int CurrentHealth => _currentHealth;

    //Movement Variables
    [SerializeField] float _speed = 5;
    [SerializeField] float _jumpForce = 5;
    [SerializeField] float _fallGravMult = 2.5f;         // Gravity while falling
    [SerializeField] float _jumpGravMult = 2f;           // Gravity while jumping
    [SerializeField] float _checkGroundRadius = 0.2f;    // Radius of circle check
    [SerializeField] float _rememberGroundedFor = 0.1f;  // How long after jumping can you jump again
    [SerializeField] int _defaultAdditionalJumps = 0;    // Extra Jumps
    [SerializeField] float _knockbackForce = 15;

    // iFrame Variables
    [SerializeField] float _iFrameDuration;
    bool _invincible;
    private float numOfFlashes = 8;
    private Color flashColor = new Color32(255,75,75,255);

    // Movement State Variables
    bool _facingRight = true;
    bool _paused = false;
    int _additionalJumps = 0;
    float _lastTimeGrounded;
    public bool _isGrounded = false;
    public bool _isAlive = true;
    public bool _isKnockedBack = false;

    // References
    [SerializeField] Transform _groundChecker;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] ParticleSystem _damagedParticles;
    [SerializeField] AudioClip _damagedSFX;
    GameObject _artHolder;
    Rigidbody2D _rb;
    Animator _animator;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        _artHolder = transform.Find("HoboSide").gameObject;
        _rb = GetComponent<Rigidbody2D>();
        _animator = transform.GetChild(0).GetComponent<Animator>();
        _facingRight = true;
    }

    public void Update()
    {
        // Restart Game
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // Exit Game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Test for getting damaged
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DecreaseHealth(1, transform.forward.x);
        }

        // No player interactivity in the tutorial
        if (_isAlive == true && _paused == false)
        {
            Move();
            Jump();
            Gravity();
            CheckIfGrounded();
        }
    }

    void Move()
    {
        if (_isKnockedBack == false)
        {
            float x = Input.GetAxisRaw("Horizontal");
            if (x != 0)
            {
                _animator.SetBool("Moving", true);
            }
            else
            {
                _animator.SetBool("Moving", false);
            }

            float moveBy = x * _speed;
            _rb.velocity = new Vector2(moveBy, _rb.velocity.y);

            if (_rb.velocity.x > 0 && !_facingRight)  // Moving right, facing left
            {
                Flip(); // Flip right
            }
            else if (_rb.velocity.x < 0 && _facingRight) // Moving left, facing right
            {
                Flip(); // Flip left
            }
        }
        else if (_isKnockedBack == true && _rb.velocity.magnitude <= 0.01f)
        {
            _isKnockedBack = false;
        }
    }

    void Jump()
    {
        // Jumps if player is on the ground or has additional jumps
        // and they haven't run out of time to call another jump (_rememberGroundedFor)
        if ((Input.GetKeyDown(KeyCode.Space)) && (_isGrounded || Time.time - _lastTimeGrounded <= _rememberGroundedFor && _additionalJumps > 0))
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
            _additionalJumps--;
            _animator.SetTrigger("Jump");
        }

    }

    void CheckIfGrounded()
    {

        // Checks for collision between the ground layer and a circle collider that is created
        Collider2D colliders = Physics2D.OverlapCircle(_groundChecker.position, _checkGroundRadius, _groundLayer);
        //Set grounded and resets player jumps
        if (colliders != null)
        {
            _isGrounded = true;
            _additionalJumps = _defaultAdditionalJumps;
            _animator.SetBool("Grounded", true);
        }
        else
        {
            // Sets _lastTimeGrounded, used to see how long it's been since player has touched the ground
            if (_isGrounded)
            {
                _lastTimeGrounded = Time.time;
            }
            // No longer grounded
            _isGrounded = false;
            _animator.SetBool("Grounded", false);
        }
    }

    // I currently have both the fall and jump multiplier set to the same value
    void Gravity()
    {
        // Gravity While Falling
        if (_rb.velocity.y < 0)
        {
            _rb.velocity += Vector2.up * Physics2D.gravity * (_fallGravMult - 1) * Time.deltaTime;
        }

        // Gravity While Jumping
        else if (_rb.velocity.y > 0 )
        {
            _rb.velocity += Vector2.up * Physics2D.gravity * (_jumpGravMult - 1) * Time.deltaTime;
        }

    }

    // Flips player
    private void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 xScale = transform.localScale;
        xScale.x *= -1;
        transform.localScale = xScale;
    }

    public void DecreaseHealth(int amount, float knockDirection)
    {
        if (_invincible == false)
        {

            // Stops setting the player's velocity directly until the knockback is resolved
            _isKnockedBack = true;

            // Replace 1 with facing direction of enemy
            _rb.velocity = new Vector2(knockDirection * _knockbackForce / 2, transform.up.y * _knockbackForce);

            _currentHealth -= amount;
            // Starts iFrame coroutine
            StartCoroutine(IFrameCoroutine(_iFrameDuration));
            // Sends out event message
            PlayerDamage?.Invoke();
            // Particles/Sound
            DamageFeedback();
        }

        if(_currentHealth <= 0)
        {
            Kill();
        }
    }

    // Duration is set to the value of _delayStartTime
    private IEnumerator IFrameCoroutine(float duration)
    {
        // Damage from occuring
        _invincible = true;

        // Timer and flash setup
        float timestamp = Time.time + duration;
        Color defaultColor = _artHolder.GetComponentInChildren<SpriteRenderer>().color;
        bool flash = false;

        // While loop for duration
        while (Time.time < timestamp)
        {
            // If flash is true, set color to flash color, otherwise set color back to default
            foreach (SpriteRenderer sr in _artHolder.GetComponentsInChildren<SpriteRenderer>())
            {
                sr.color = flash ? flashColor : defaultColor;
            }
            flash = !flash;
            yield return new WaitForSeconds(duration / numOfFlashes);
        }

        // Reset values after duration
        foreach (SpriteRenderer sr in _artHolder.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.color = defaultColor;
        }
        _invincible = false;
    }

    private void DamageFeedback()
    {
        //particles
        if (_damagedParticles != null)
        {
            _damagedParticles = Instantiate(_damagedParticles, transform.position + new Vector3(0, 1, 0), Quaternion.Euler(-90, 0, 0));
        }
        //audio
        if (_damagedSFX != null)
        {
            AudioHelper.PlayClip2D(_damagedSFX, 1f);
        }
    }

    public void Kill()
    {
        gameObject.SetActive(false);
    }

}
