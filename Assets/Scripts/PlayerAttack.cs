using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float _attackDelayTime = 0;
    [SerializeField] private float _maxAttackDelayTime;

    [SerializeField] private Transform _attackPos;
    [SerializeField] LayerMask Enemies;

    [SerializeField] AudioClip[] _swingSFX;

    Animator _animator;
    AudioManager _am;

    private void Awake()
    {
        _am = FindObjectOfType<AudioManager>();
        _animator = transform.GetChild(0).GetComponent<Animator>();
    }

    void Update()
    {
        if (_attackDelayTime <= 0)
        {
            // Left Mouse Button, Vertical Attack
            if (Input.GetMouseButtonDown(0))
            {
                // Start animation, set direction, start attack delay count
                _animator.SetTrigger("Attack");
                _animator.SetInteger("AttackDir",0);
                _attackDelayTime = _maxAttackDelayTime;
                _am.Play("Swing");
                //AudioHelper.PlayClip2D(_swingSFX[Random.Range(0,3)], .25f);
            }
            // Right Mouse Button, Horizontal Attack
            else if (Input.GetMouseButtonDown(1))
            {
                // Start animation, set direction, start attack delay count
                _animator.SetTrigger("Attack");
                _animator.SetInteger("AttackDir", 1);
                _attackDelayTime = _maxAttackDelayTime;
                _am.Play("Swing");
                //AudioHelper.PlayClip2D(_swingSFX[Random.Range(0, 3)], .25f);
            }
        }
        else
        {
            _attackDelayTime -= Time.deltaTime;
        }
    }
}
