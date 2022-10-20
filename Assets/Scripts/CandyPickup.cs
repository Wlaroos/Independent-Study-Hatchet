using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CandyPickup : MonoBehaviour
{

    // THIS SCRIPT IS ON THE CANDY PARTICLE PREFAB
    //
    // ALLOWS INTERACTION WITH CANDY PARTICLES AND PLAYER

    [SerializeField] HUDController _HUDref;

    [SerializeField] int _candyValue = 1;

    ParticleSystem ps;

    List<ParticleSystem.Particle> _particles = new List<ParticleSystem.Particle>();

    private void Awake()
    {
        ps = transform.GetComponent<ParticleSystem>();
        _HUDref = FindObjectOfType<HUDController>();
        GetComponent<ParticleSystem>().trigger.SetCollider(0, _HUDref.gameObject.transform);
    }


    // When player enters the trigger of the candy particle
    private void OnParticleTrigger()
    {
        // Gets how many particles that were touched
        int triggeredParticles = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, _particles);

        // For each particle that the player touched
        for (int i = 0; i < triggeredParticles; i++)
        {
            // Local reference for a particle in the list
            ParticleSystem.Particle p = _particles[i];
            // Changes lifetime of the particle
            p.remainingLifetime = 0;
            // Add amount to player's candy count
            _HUDref.AddCandy(_candyValue);
            // Applies local referece changes to the particle in the list
            _particles[i] = p;
        }

        // Apply changes to the particle system using the new values from the list
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, _particles);

    }
}
