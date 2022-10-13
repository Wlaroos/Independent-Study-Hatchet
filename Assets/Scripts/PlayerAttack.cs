using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float _attackDelayTime = 0;
    [SerializeField] private float _maxAttackDelayTime;

    [SerializeField] private Transform _attackPos;
    [SerializeField] LayerMask Enemies;
    [SerializeField] private float _attackRangeX;
    [SerializeField] private float _attackRangeY;

    //bool _triggerEnabled = false;

    //BoxCollider2D _axeCollider;
    Animator _animator;

    private void Awake()
    {
        _animator = transform.GetChild(0).GetComponent<Animator>();
        //_axeCollider = _attackPos.gameObject.GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (_attackDelayTime <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {

                _animator.SetTrigger("Attack");
                _animator.SetInteger("AttackDir",0);
                _attackDelayTime = _maxAttackDelayTime;

                /*Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(_attackPos.position, new Vector2(_attackRangeX, _attackRangeY), 0, Enemies);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    if (enemiesToDamage[i].GetComponent<EnemyBase>() != null && _triggerEnabled == true)
                    {
                        enemiesToDamage[i].GetComponent<EnemyBase>().TakeDamage(0);
                    }
                    _attackDelayTime = _maxAttackDelayTime;
                }*/
            }

            else if (Input.GetMouseButtonDown(1))
            {

                _animator.SetTrigger("Attack");
                _animator.SetInteger("AttackDir", 1);
                _attackDelayTime = _maxAttackDelayTime;

                /*    Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(_attackPos.position, new Vector2(_attackRangeX, _attackRangeY), 0, Enemies);
                    for (int i = 0; i < enemiesToDamage.Length; i++)
                    {
                        if (enemiesToDamage[i].GetComponent<EnemyBase>() != null && _triggerEnabled == true)
                        {
                            enemiesToDamage[i].GetComponent<EnemyBase>().TakeDamage(1);
                        }
                        _attackDelayTime = _maxAttackDelayTime;
                    }*/
            }
        }
        else
        {
            _attackDelayTime -= Time.deltaTime;
        }
    }

 /*   public void TriggerToggle()
    {
        _triggerEnabled = !_triggerEnabled;
        if(_triggerEnabled)
        {
            _axeCollider.enabled = true;
        }
        else
        {
            _axeCollider.enabled = false;
        }
    }*/

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_attackPos.position, new Vector3(_attackRangeX, _attackRangeY, 1));
    }
}
