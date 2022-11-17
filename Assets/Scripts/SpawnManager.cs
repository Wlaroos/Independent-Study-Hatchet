﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] PlayerController _playerRef;
    [SerializeField] GameObject[] _enemies;
    [SerializeField] float _spawnInterval;
    [SerializeField] float[] _spawnWeights;
    [DraggablePoint] public Vector3[] _spawnPoints;

    private void Start()
    {
        StartCoroutine(SpawnEnemy(_spawnInterval, _enemies[GetRandomWeightedIndex(_spawnWeights)]));
    }

    private IEnumerator SpawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, _spawnPoints[Random.Range(0, _spawnPoints.Length)], Quaternion.identity);
        newEnemy.GetComponent<EnemyBase>().SetPlayerRef(_playerRef);
        if (newEnemy.name.Contains("Witch"))
        {
            newEnemy.transform.position = new Vector3(newEnemy.transform.position.x, -6.5f, 0);
        }
        else { newEnemy.transform.position = new Vector3(newEnemy.transform.position.x, -10f, 0); }
        StartCoroutine(SpawnEnemy(_spawnInterval, _enemies[GetRandomWeightedIndex(_spawnWeights)]));
    }

    public int GetRandomWeightedIndex(float[] weights)
    {
        if (weights == null || weights.Length == 0) return -1;

        float w;
        float t = 0;
        int i;
        for (i = 0; i < weights.Length; i++)
        {
            w = weights[i];

            if (float.IsPositiveInfinity(w))
            {
                return i;
            }
            else if (w >= 0f && !float.IsNaN(w))
            {
                t += weights[i];
            }
        }

        float r = Random.value;
        float s = 0f;

        for (i = 0; i < weights.Length; i++)
        {
            w = weights[i];
            if (float.IsNaN(w) || w <= 0f) continue;

            s += w / t;
            if (s >= r) return i;
        }

        return -1;
    }


}
