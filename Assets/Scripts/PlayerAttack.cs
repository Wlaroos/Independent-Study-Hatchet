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


    void Update()
    {
        if (_attackDelayTime <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(_attackPos.position, new Vector2(_attackRangeX, _attackRangeY), 0, Enemies);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    if (enemiesToDamage[i].GetComponent<Enemy>() != null)
                    {
                        enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(0);
                    }
                    else
                    {
                        enemiesToDamage[i].GetComponent<Enemy2>().TakeDamage(0);
                    }
                    _attackDelayTime = _maxAttackDelayTime;
                }
            }

            else if (Input.GetMouseButtonDown(1))
            {
                Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(_attackPos.position, new Vector2(_attackRangeX, _attackRangeY), 0, Enemies);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    if (enemiesToDamage[i].GetComponent<Enemy>() != null)
                    {
                        enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(1);
                    }
                    else
                    {
                        enemiesToDamage[i].GetComponent<Enemy2>().TakeDamage(1);
                    }
                    _attackDelayTime = _maxAttackDelayTime;
                }
            }
        }
        else
        {
            _attackDelayTime -= Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_attackPos.position, new Vector3(_attackRangeX, _attackRangeY, 1));
    }
}
