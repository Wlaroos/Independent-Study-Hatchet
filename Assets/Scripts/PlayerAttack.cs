using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float _attackDelayTime = 0;
    [SerializeField] private float _maxAttackDelayTime;

    [SerializeField] private Transform _attackPos;
    [SerializeField] LayerMask Enemies;

    Animator _animator;

    private void Awake()
    {
        _animator = transform.GetChild(0).GetComponent<Animator>();
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
            }
            else if (Input.GetMouseButtonDown(1))
            {
                _animator.SetTrigger("Attack");
                _animator.SetInteger("AttackDir", 1);
                _attackDelayTime = _maxAttackDelayTime;
            }
        }
        else
        {
            _attackDelayTime -= Time.deltaTime;
        }
    }
}
