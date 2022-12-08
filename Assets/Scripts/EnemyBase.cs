using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.U2D.Animation;


public abstract class EnemyBase : MonoBehaviour
{

    // Movement/Health Variables
    [SerializeField] protected float _maxSpeed = 5f;
    protected float _speed;
    protected Vector3 _direction;   
    [SerializeField] protected int _knockbackForce = 150;
    [SerializeField] protected int _maxHealth = 1;
    private int _currentHealth;
    [SerializeField] protected int _contactDamage = 1;

    // Dazed and iFrame variables
    [SerializeField] protected float _startDazedTime = 0.5f;
    private bool _dazed = false;
    private float numOfFlashes = 6;
    private Color flashColor = new Color32(255, 75, 75, 255);

    // Prefab variables
    [SerializeField] GameObject _horizontal;
    [SerializeField] GameObject _vertical;
    [SerializeField] ParticleSystem _candyParticle;

    protected GameObject _playerRef;
    protected GameObject _crateRef;
    protected GameObject _objectToFollowRef;
    protected static EnemyBase _isAttackingCrate;

    // Component/Object Variables
    protected Animator _anim;
    protected GameObject _artHolder;
    protected Rigidbody2D _rb;
    protected GameObject _arrowHolder;
    private List<GameObject> _arrowList = new List<GameObject>();

    [SerializeField] SpriteLibraryAsset[] _skins;
    SpriteLibraryAsset _currentSkin;

    protected AudioManager _am;
    [SerializeField] AudioClip[] _damagedSFX;
    [SerializeField] AudioClip[] _deathSFX;

    protected virtual void Awake()
    {
        _am = FindObjectOfType<AudioManager>();
        if(_maxHealth > 1) _maxHealth = Random.Range(_maxHealth - 1, _maxHealth + 1);
        _currentHealth = _maxHealth;
        _speed = _maxSpeed;
        _artHolder = transform.GetChild(0).gameObject;
        _rb = GetComponent<Rigidbody2D>();
        _arrowHolder = transform.Find("ArrowHolder").gameObject;
        _anim = _artHolder.GetComponent<Animator>();

        if(_skins.Length > 0)
        {
            _currentSkin = _skins[Random.Range(0, _skins.Length)];
            _artHolder.GetComponent<SpriteLibrary>().spriteLibraryAsset = _currentSkin;
        }
    }

    private void Start()
    {
        // Create arrows as soon as game starts
        AddArrows();
        Move();
        StartCoroutine(SpawnDelay());
        StartCoroutine(DistanceChecker());
    }

    protected virtual void Update()
    {
        _direction = new Vector3(_objectToFollowRef.transform.position.x - transform.position.x, 0, 0);
    }

    private void FixedUpdate()
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
        Vector3 scale = transform.localScale;

        if (_objectToFollowRef.transform.position.x > transform.position.x)
        {
            scale.x = -1;
            _arrowHolder.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            scale.x = 1;
            _arrowHolder.transform.localScale = new Vector3(1, 1, 1);
        }

        transform.localScale = scale;

        // Checks for collision between the ground layer and a circle collider that is created
        LayerMask mask = LayerMask.GetMask("Ground");
        LayerMask mask2 = LayerMask.GetMask("Ground(Objects)");
        Collider2D colliders = Physics2D.OverlapCircle(transform.position + new Vector3(0, -.88f, 0), 1f, mask);
        Collider2D colliders2 = Physics2D.OverlapCircle(transform.position + new Vector3(0, -.88f, 0), 1f, mask2);

        // Enemy GroundCheck
        if (colliders != null || colliders2 != null)
        {
            // Check if enemy is close to the crate
            if (_objectToFollowRef == _crateRef && Mathf.Abs(Vector2.Distance(_objectToFollowRef.transform.position, transform.position)) < 2)
            {
                _rb.velocity = new Vector2(0, -10);

                // If no one is attacking the crate, set itself as the one attacking the crate
                if (_isAttackingCrate == null)
                {
                    var test = GameObject.FindGameObjectsWithTag("Enemy");
                    for (int i = 0; i < test.Length; i++)
                    {
                        // Sets the static variables for the enemies that are in the scene
                        test[i].GetComponent<EnemyBase>().SetAttacker(this);
                    }
                }
            }

            // Normal Movement
            else
            {
                _rb.velocity = new Vector2(_direction.normalized.x * _speed, -1f * _speed);
            }

        }

        // If ground check fails, remove x velocity
        else
        {
            _rb.velocity = new Vector2(0, -10);
        }
    }

    protected virtual void Attack()
    {
        _anim.Play(this.GetType().ToString() + "Attack");
        _anim.SetBool("isAttacking", true);
        Invoke("ResetBool", _anim.GetCurrentAnimatorStateInfo(0).length);
    }

    private void ResetBool()
    {
        _anim.SetBool("isAttacking", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        PlayerController playerRef = collision.gameObject.transform.GetComponent<PlayerController>();
        CandyCrate crateRef = collision.gameObject.transform.GetComponent<CandyCrate>();

        // If they touch the player, decrease health and give direction for knockback (Direction enemy is facing)
        if (playerRef != null && _anim.GetBool("isAttacking") == true)
        {
            int dir;
            if (transform.forward.x == 1) dir = 1; else dir = -1;

            playerRef.DecreaseHealth(_contactDamage, dir);
        }

        if (crateRef != null && _anim.GetBool("isAttacking") == true)
        {
            crateRef.TakeDamage(_contactDamage);
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

        else if (direction == 0 && _arrowList[0].GetComponent<SpriteRenderer>().color != Color.magenta)
        {
            ScreenShake.ShakeOnce(.2f, 1f);
        }
        else if (direction == 1 && _arrowList[0].GetComponent<SpriteRenderer>().color != Color.cyan)
        {
            ScreenShake.ShakeOnce(.2f, 1f);
        }

    }

    protected virtual void DamageFeedback()
    {
        //if (_damagedSFX != null) { AudioHelper.PlayClip2D(_damagedSFX[Random.Range(0, 3)], .25f); }

        // Dazed coroutine, knockback and decrease health
        StartCoroutine(IFrameCoroutine(_startDazedTime));
        _rb.AddForce(new Vector2(_knockbackForce * transform.localScale.x, 0));
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
            if (_skins.Length > 0) verticalHalves.transform.GetComponentInChildren<SpriteLibrary>().spriteLibraryAsset = _currentSkin;
        }
        else if (direction == 1)
        {
            GameObject horizontalHalves = Instantiate(_horizontal, transform.position, transform.rotation);
            if (_skins.Length > 0) horizontalHalves.transform.GetComponentInChildren<SpriteLibrary>().spriteLibraryAsset = _currentSkin;
        }
        ParticleSystem candy = Instantiate(_candyParticle, transform.position + new Vector3(0, 0, -.05f), _candyParticle.transform.rotation);
        _am.Play("EnemyDeath");
        //if (_deathSFX != null) { AudioHelper.PlayClip2D(_deathSFX[Random.Range(0, 2)], 1f); }
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
        _dazed = false;
        _speed = _maxSpeed;
    }

    private IEnumerator SpawnDelay()
    {
        // Stops movement and damage from occuring
        _speed = 0;
        _dazed = true;
        if(!gameObject.name.Contains("Witch")) _rb.velocity = new Vector2(0, -1);

        // Waits until enemy hits ground
        yield return new WaitUntil(() => _rb.velocity.y == 0);

        _dazed = false;
        _speed = _maxSpeed;
        // Start idle animation at random location so enemies don't move in sync
        _anim.Play(0, 0, Random.value);
    }

    // Checks how close the enemy is to the crate and the player every 2 seconds
    private IEnumerator DistanceChecker()
    {
        yield return new WaitForSeconds(2f);

        // Follow Player
        if(Vector2.Distance(transform.position, _playerRef.transform.position) <= Vector2.Distance(transform.position, _crateRef.transform.position))
        {
            _objectToFollowRef = _playerRef;
        }

        // If no one is attacking the crate, follow the crate
        else
        {
            if (_isAttackingCrate == null)
            {
                _objectToFollowRef = _crateRef;
            }
        }

        StartCoroutine(DistanceChecker());
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

    // Sets references from the SpawnManager
    public void SetRefs(PlayerController player, CandyCrate crate)
    {
        _playerRef = player.gameObject;
        _crateRef = crate.gameObject;
        _objectToFollowRef = _playerRef;
    }

    // Base class is random between vertical and horizontal. Can be overwritten for specific enemies
    protected virtual int ArrowDirection()
    {
        return Random.Range(0, 2);
    }

    // Get method for the EnemyAttackRange script
    public EnemyBase GetAttacker()
    {
        return _isAttackingCrate;
    }

    // Changes the static variable for the items that are already in the scene
    public void SetAttacker(EnemyBase enemy)
    {
        _isAttackingCrate = enemy;
    }
}