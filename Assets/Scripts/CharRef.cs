using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharRef : MonoBehaviour
{
    [SerializeField] PlayerController _player;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
        GetComponent<ParticleSystem>().trigger.SetCollider(0, _player.gameObject.GetComponent<CapsuleCollider2D>());
    }
}
