using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;

public class Witch : EnemyBase
{

    [SerializeField] SpriteLibraryAsset[] _skins;

    protected override int ArrowDirection()
    {
        return base.ArrowDirection();
    }

    protected override void Awake()
    {
        base.Awake();

        _artHolder.GetComponent<SpriteLibrary>().spriteLibraryAsset = _skins[Random.Range(0,_skins.Length)];
    }

    protected override void Move()
    {
        base.Move();
    }

    protected override void Attack()
    {
        base.Attack();
    }
}
