using System.Collections;
using UnityEngine;
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
    [SerializeField] float _knockbackForce = 10;

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

    // References
    [SerializeField] Transform _groundChecker;
    [SerializeField] LayerMask _groundLayer;

    ParticleSystem _runParticles;
    [SerializeField] GameObject _landedParticles;
    bool hasPoofed = false;

    [SerializeField] ParticleSystem _damagedParticles;
    [SerializeField] AudioClip _damagedSFX;

    GameObject _artHolder;
    Rigidbody2D _rb;
    Animator _animator;
    AudioManager _am;

    private void Awake()
    {
        _am = FindObjectOfType<AudioManager>();
        _currentHealth = _maxHealth;
        _runParticles = transform.GetChild(2).GetComponent<ParticleSystem>();
        _artHolder = transform.GetChild(0).gameObject;
        _rb = GetComponent<Rigidbody2D>();
        _animator = _artHolder.GetComponent<Animator>();
        _facingRight = true;
    }

    public void Update()
    {
        // Stops player interactivity if paused or dead
        if (_isAlive == true && _paused == false)
        {
            Move();
            Jump();
            Gravity();
            CheckIfGrounded();
        }

        // Player control is taken away after being hit and given back after stopping on the ground
        if (_paused == true && _rb.velocity.magnitude <= 0.01f)
        {
            _paused = false;
        }
    }

    void Move()
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

    void Jump()
    {
        // Jumps if player is on the ground or has additional jumps
        // and they haven't run out of time to call another jump (_rememberGroundedFor)
        if ((Input.GetKeyDown(KeyCode.Space)) && (_isGrounded || Time.time - _lastTimeGrounded <= _rememberGroundedFor && _additionalJumps > 0))
        {
            _am.Play("Jump");
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
            var emission = _runParticles.emission;
            emission.enabled = true;
            LandPoof();
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
            var emission = _runParticles.emission;
            emission.enabled = false;
            hasPoofed = false;
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
        _runParticles.transform.localScale = xScale;
    }

    public void DecreaseHealth(int amount, float knockDirection)
    {
        if (_invincible == false)
        {

            // Stops setting the player's velocity directly until the knockback is resolved (Hit ground)
            _paused = true;

            // Knocked back into the air and in the x direction the enemy hit you from
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
            ParticleSystem DamagedParticles = Instantiate(_damagedParticles, transform.position + new Vector3(0, 1, 0), Quaternion.Euler(-90, 0, 0));
        }
        //audio
        /*        if (_damagedSFX != null)
                {
                    AudioHelper.PlayClip2D(_damagedSFX, 1f);
                }*/

        _am.Play("PlayerDamage");

        ScreenShake.ShakeOnce(.5f, 2.5f);
    }

    private void Kill()
    {
        _am.Play("PlayerDeath");
        ScreenShake.ShakeOnce(.75f, 5f);
        gameObject.SetActive(false);
    }

    private void LandPoof()
    {
        if (!hasPoofed)
        {
            // Connects the particle to the player, sets position, then disconnects it so it doesn't follow the player
            _landedParticles.transform.SetParent(transform);
            _landedParticles.transform.localPosition = new Vector3(-.2f, -1f, 0);
            _landedParticles.transform.SetParent(null);
            _am.Play("Landing");

            // Gameobject has 3 children that are all particle systems. Each one needs to be played.
            foreach (Transform child in _landedParticles.transform)
            {
                ParticleSystem ps = child.GetComponent<ParticleSystem>();
                ps.Play();
            }
        }
        hasPoofed = true;
    }

}
