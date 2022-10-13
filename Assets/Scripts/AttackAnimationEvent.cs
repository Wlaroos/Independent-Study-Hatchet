using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationEvent : MonoBehaviour
{

    [SerializeField] BoxCollider2D _axeTriggerRef;
     AxeController _axeControllerRef;
     Animator _animRef;

    bool _triggerEnabled = false;

    private void Awake()
    {
        _animRef = GetComponent<Animator>();
        _axeControllerRef = _axeTriggerRef.GetComponent<AxeController>();
    }

    void ToggleAxe()
    {
        _triggerEnabled = !_triggerEnabled;

        if (_triggerEnabled)
        {
            _axeControllerRef._attackDir = _animRef.GetInteger("AttackDir");
            _axeTriggerRef.enabled = true;
        }
        else
        {
            _axeTriggerRef.enabled = false;
        }
    }
}
