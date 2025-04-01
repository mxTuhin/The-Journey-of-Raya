using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private List<EnemySystem> _enemies = new List<EnemySystem>();
    
    private static EnemyManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        EnemySystem.OnEnemySpawn += AddEnemy;
        EnemySystem.OnEnemyDeath += RemoveEnemy;
    }

    private void OnDisable()
    {
        EnemySystem.OnEnemySpawn -= AddEnemy;
        EnemySystem.OnEnemyDeath -= RemoveEnemy;
    }
    
    private void AddEnemy(EnemySystem enemy)
    {
        if(!_enemies.Contains(enemy))
            _enemies.Add(enemy);
    }
    
    private void RemoveEnemy(EnemySystem enemy)
    {
        if(_enemies.Contains(enemy))
            _enemies.Remove(enemy);
    }
    
    public List<EnemySystem> GetEnemies()
    {
        return _enemies;
    }
    
    public void ResetEnemies()
    {
        _enemies.Clear();
    }
    
    public EnemySystem GetNearestEnemy(Vector3 position)
    {
        return GetNearestEnemy(position, _enemies);
    }
    
    public List<EnemySystem> GetMultipleNearestEnemies(Vector3 position, int number)
    {
        List<EnemySystem> nearestEnemies = new List<EnemySystem>();
        List<EnemySystem> tempEnemies = new List<EnemySystem>(_enemies);
        
        for (int i = 0; i < number; i++)
        {
            EnemySystem nearestEnemy = GetNearestEnemy(position, tempEnemies);
            if (nearestEnemy != null)
            {
                nearestEnemies.Add(nearestEnemy);
                tempEnemies.Remove(nearestEnemy);
            }
        }
        
        return nearestEnemies;
    }
    
    private EnemySystem GetNearestEnemy(Vector3 position, List<EnemySystem> enemies)
    {
        EnemySystem nearestEnemy = null;
        float minDistance = float.MaxValue;
        
        foreach (EnemySystem enemy in enemies)
        {
            float distance = Vector3.Distance(position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = enemy;
            }
        }
        
        return nearestEnemy;
    }
    
    public static EnemyManager GetInstance()
    {
        return instance;
    }
}
