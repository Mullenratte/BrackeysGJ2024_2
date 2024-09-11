using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    public List<EnemyBase> Enemies { get; private set; }
    public int maxEnemies;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }

        Enemies = new List<EnemyBase>();    
    }

    public void AddEnemyToEnemies(EnemyBase enemy) {
        Enemies.Add(enemy);
    }

    public void TryRemoveEnemyFromEnemies(EnemyBase enemy) {
        if (Enemies.Contains(enemy)) {
            Enemies.Remove(enemy);
        }
    }

    public bool CanSpawn() {
        return Enemies.Count < maxEnemies ? true : false;
    }


}
