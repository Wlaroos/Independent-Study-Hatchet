using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharRef : MonoBehaviour
{
    [SerializeField] PlayerData _player;

    ParticleSystem ps;

    List<ParticleSystem.Particle> _particles = new List<ParticleSystem.Particle>();

    private void Awake()
    {
        ps = transform.GetComponent<ParticleSystem>();
        _player = FindObjectOfType<PlayerData>();
        GetComponent<ParticleSystem>().trigger.SetCollider(0, _player.gameObject.transform);
    }

    private void OnParticleTrigger()
    {
        int triggeredParticles = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, _particles);

        for (int i = 0; i < triggeredParticles; i++)
        {
            ParticleSystem.Particle p = _particles[i];
            p.remainingLifetime = 0;
            _player.Candy(1);
            _particles[i] = p;
        }

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, _particles);

    }
}
