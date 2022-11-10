public class Werewolf : EnemyBase
{

    protected override int ArrowDirection()
    {
        return base.ArrowDirection();
    }

    protected override void Move()
    {
        base.Move();
    }

    protected override void Attack()
    {
        base.Attack();
    }

    protected override void DamageFeedback()
    {
        base.DamageFeedback();
        _am.Play("WerewolfDamage");
    }
}
