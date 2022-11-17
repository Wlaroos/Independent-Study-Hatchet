using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchAttackAnimationEvent : MonoBehaviour
{

    [SerializeField] WitchOrb _orbRef;
    [SerializeField] Transform _throwPos;

    private void Orb()
    {
        Instantiate(_orbRef, _throwPos.position, Quaternion.identity);
    }

}
