using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemiesController : MonoBehaviour
{
    [SerializeField] private List<Enemy> enemies;
    [SerializeField] private Transform spawnRoot;
    [SerializeField] private float minDelay;
    [SerializeField] private float maxDelay;
    
    private Coroutine routine;

    private void OnEnable()
    {
        Activate();
    }

    private void OnDisable()
    {
        Deactivate();
    }

    public void Activate()
    {
        if (routine != null)
        {
            StopCoroutine(routine);
        }
        
        routine = StartCoroutine(EnemiesCreateRoutine());
    }
    
    public void Deactivate()
    {
        if (routine != null)
        {
            StopCoroutine(routine);
        }
    }

    private IEnumerator EnemiesCreateRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
            CreateEnemy();
        }
    }

    private void CreateEnemy()
    {
        Instantiate(enemies[Random.Range(0, enemies.Count)], spawnRoot);
    }
}
