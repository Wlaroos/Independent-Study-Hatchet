using UnityEngine;
public class Witch : EnemyBase
{

    protected override int ArrowDirection()
    {
        return base.ArrowDirection();
    }

    protected override void Update()
    {
        _direction = new Vector3((_playerRef.transform.position.x + 2) - transform.position.x, 0, 0);
    }

    protected override void Move()
    {
        Vector3 scale = transform.localScale;

        if (_playerRef.transform.position.x > transform.position.x)
        {
            scale.x = -1;
        }
        else 
        { 
            scale.x = 1;
        }

        transform.localScale = scale;

        if ((_playerRef.transform.position.x + 3.5 - transform.position.x) <= 0.05f && scale.x == 1)
        {
            _rb.MovePosition(transform.position + _direction.normalized * _speed * Time.fixedDeltaTime);
        }
        else if ((_playerRef.transform.position.x - 3.5 - transform.position.x) >= 0.05f && scale.x == -1)
        {
            _rb.MovePosition(transform.position + _direction.normalized * _speed * Time.fixedDeltaTime);
        }
    }

    protected override void Attack()
    {
        base.Attack();
    }

    protected override void DamageFeedback()
    {
        base.DamageFeedback();
        _am.Play("WitchDamage");
    }
}
