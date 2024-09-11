using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs;

    public EnemyBase SpawnEnemy(Vector3 spawnPos, GameObject enemyPrefab) {
        GameObject prefab = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        prefab.TryGetComponent(out EnemyBase enemy);
        return enemy;
    }

    public EnemyBase SpawnRandomEnemy(Vector3 spawnPos) {
        int rnd = Random.Range(0, enemyPrefabs.Length);

        GameObject prefab = Instantiate(enemyPrefabs[rnd], spawnPos, Quaternion.identity);
        prefab.TryGetComponent(out EnemyBase enemy);
        return enemy;
    }
}
