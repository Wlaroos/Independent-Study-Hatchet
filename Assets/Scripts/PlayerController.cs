using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _speed = 5;
    [SerializeField] float _jumpForce = 5;
    [SerializeField] float _fallMultiplier = 2.5f;
    [SerializeField] float _lowJumpMultiplier = 2f;
    [SerializeField] float _checkGroundRadius = 0.2f;
    [SerializeField] float _rememberGroundedFor = 0.1f;
    [SerializeField] int _defaultAdditionalJumps = 0;

    bool _facingRight = true;
    bool _paused = false;
    int _additionalJumps = 0;
    float _lastTimeGrounded;
    public bool _isGrounded = false;
    public bool _isAlive = true;

    [SerializeField] Transform _groundChecker;
    [SerializeField] LayerMask _groundLayer;
    Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _facingRight = true;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        // No player interactivity in the tutorial
        if (_isAlive == true && _paused == false)
        {
            Move();
            Jump();
            BetterJump();
            CheckIfGrounded();
        }
    }

    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        if(x != 0)
        {
            transform.GetChild(0).GetComponent<Animator>().SetBool("Moving", true);
        }
        else
        {
            transform.GetChild(0).GetComponent<Animator>().SetBool("Moving", false);
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
        if ((Input.GetKeyDown(KeyCode.Space)) && (_isGrounded || Time.time - _lastTimeGrounded <= _rememberGroundedFor && _additionalJumps > 0))
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
            _additionalJumps--;
            transform.GetChild(0).GetComponent<Animator>().SetTrigger("Jump");
        }

    }

    void CheckIfGrounded()
    {
        Collider2D colliders = Physics2D.OverlapCircle(_groundChecker.position, _checkGroundRadius, _groundLayer);
        if (colliders != null)
        {
            _isGrounded = true;
            _additionalJumps = _defaultAdditionalJumps;
            transform.GetChild(0).GetComponent<Animator>().SetBool("Grounded", true);
        }
        else
        {
            if (_isGrounded)
            {
                _lastTimeGrounded = Time.time;
            }
            _isGrounded = false;
            transform.GetChild(0).GetComponent<Animator>().SetBool("Grounded", false);
        }
    }

    // Jump higher when holding space
    void BetterJump()
    {
        if (_rb.velocity.y < 0)
        {
            _rb.velocity += Vector2.up * Physics2D.gravity * (_fallMultiplier - 1) * Time.deltaTime;
        }
        else if (_rb.velocity.y > 0 )
        {
            _rb.velocity += Vector2.up * Physics2D.gravity * (_lowJumpMultiplier - 1) * Time.deltaTime;
        }

    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

}
