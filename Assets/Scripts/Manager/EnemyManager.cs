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
        EnemySystem nearestEnemy = null;
        float minDistance = float.MaxValue;
        
        foreach (EnemySystem enemy in _enemies)
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
