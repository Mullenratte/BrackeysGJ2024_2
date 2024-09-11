using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] float maxSpawnRadius, minSpawnInterval, maxSpawnInterval;
    const float MINDISTTOSOLIDOBJECT = 6f;



    Vector3 spawnPos;

    private void Start() {
        ProceduralGenerator.instance.OnGenerationPointShifted += ProceduralGenerator_OnGenerationPointShifted;
        UpdateSpawnPos();

        StartCoroutine(SpawnEnemiesPeriodicallyAtRandomBetween(minSpawnInterval, maxSpawnInterval, enemyPrefabs));
    }

    private void ProceduralGenerator_OnGenerationPointShifted(object sender, ProceduralGenerator.OnGenerationPointShiftedEventArgs e) {
        Vector3 spacialDiff = e.newPos - e.oldPos;

        transform.position += spacialDiff;
        UpdateSpawnPos();
    }

    IEnumerator SpawnEnemyOnceAtRandomBetween(float minSeconds, float maxSeconds, GameObject enemyPrefab) {
        UpdateSpawnPos();
        float rnd = Random.Range(minSeconds, maxSeconds);
        yield return new WaitForSeconds(rnd);

        SpawnEnemy(enemyPrefab);
    }

    IEnumerator SpawnEnemiesPeriodicallyAtRandomBetween(float minSeconds, float maxSeconds, GameObject[] enemyPrefabs) {
        while (true) {
            float rnd = Random.Range(minSeconds, maxSeconds);
            Debug.Log("waiting for " + rnd + " seconds...");
            yield return new WaitForSeconds(rnd);
            UpdateSpawnPos();
            SpawnRandomEnemy(enemyPrefabs);
        }
    }

    private EnemyBase SpawnEnemy(GameObject enemyPrefab) {
        if (EnemyManager.instance.CanSpawn()) {
            GameObject prefab = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            prefab.TryGetComponent(out EnemyBase enemy);
            EnemyManager.instance.AddEnemyToEnemies(enemy);
            return enemy;
        }
        return null;
    }

    private EnemyBase SpawnRandomEnemy(GameObject[] enemyPrefabs) {
        if (EnemyManager.instance.CanSpawn()) {
            int rnd = Random.Range(0, enemyPrefabs.Length);

            GameObject prefab = Instantiate(enemyPrefabs[rnd], spawnPos, Quaternion.identity);
            prefab.TryGetComponent(out EnemyBase enemy);
            EnemyManager.instance.AddEnemyToEnemies(enemy);
            return enemy;
        }
        return null;
    }

    private void UpdateSpawnPos() {
        spawnPos = transform.position + Random.onUnitSphere * maxSpawnRadius;
        if (Physics.SphereCast(spawnPos, MINDISTTOSOLIDOBJECT, Vector3.zero, out RaycastHit hit)) {
            Debug.Log("cant spawn an enemy at " + spawnPos + "! Space is obstructed.");
            UpdateSpawnPos();
        }
    }
}
