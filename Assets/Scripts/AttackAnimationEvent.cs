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


    // This is called twice during the attack animation through animation events 
    // Because it's called twice it will always turn on then off, and then it'll be ready for the next call
    void ToggleAxe()
    {
        _triggerEnabled = !_triggerEnabled;

        if (_triggerEnabled)
        {
            // Get the direction that was passed through the animation and assign it to the variable in the AxeController script
            _axeControllerRef._attackDir = _animRef.GetInteger("AttackDir");
            // Enable the Axe's BoxCollider2D
            _axeTriggerRef.enabled = true;
        }
        else
        {
            // Disable the Axe's BoxCollider2D
            _axeTriggerRef.enabled = false;
        }
    }
}
