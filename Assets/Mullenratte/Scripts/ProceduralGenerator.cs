using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerator : MonoBehaviour
{
    public static ProceduralGenerator instance;

    [Header("General")]
    public float minSpawnRadius;
    public float maxSpawnRadius;
    public Transform generationPoint;
    private float generationPointDistanceX = 250f;

    [Header("Environment")]
    [SerializeField] GameObject[] environmentPrefabs;
    [SerializeField] int minEnvironmentObjects;
    [SerializeField] int maxEnvironmentObjects;
    [SerializeField] float minDistanceToObjects;
    private List<GameObject> environmentObjPool;
    [Header("Enemies")]
    [SerializeField] GameObject[] enemyPrefabs;
    [Header("Resources")]
    [SerializeField] GameObject[] resourcePrefabs;



    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        GenerateNewSection(-3*maxSpawnRadius);
        GenerateNewSection(-2*maxSpawnRadius);
        GenerateNewSection(-maxSpawnRadius);
        GenerateNewSection(0);
        environmentObjPool = new List<GameObject>();
    }


    private void Update() {
        if (Platform.instance.procGenTriggerPoint.position.x >= generationPoint.position.x) {
            GenerateNewSection(maxSpawnRadius);
            generationPoint.position += Vector3.right * generationPointDistanceX;
        }
    }

    private void GenerateNewSection(float spawnPosXOffset) {
        int rnd = Random.Range(minEnvironmentObjects, maxEnvironmentObjects + 1);
        for (int i = 0; i < rnd; i++) {
            GenerateEnvironment(spawnPosXOffset);
        }

    }

    private void GenerateEnvironment(float spawnPosXOffset) {
        int prefabRnd = Random.Range(0, environmentPrefabs.Length);

        float distanceOffsetRnd = Random.Range(minSpawnRadius, maxSpawnRadius);
        /* get a random position on a sphere around the platform center
         * within a random radius between the min and max allowed spawn radius.
         * Then, it is also offset on the X-axis by the maximum spawn radius (because there's no real need to spawn objects behind the platform)
        */
        Vector3 spawnPosRnd = Random.onUnitSphere * distanceOffsetRnd + Platform.instance.procGenTriggerPoint.position + Vector3.right * spawnPosXOffset;   

        if (Physics.SphereCast(spawnPosRnd, minDistanceToObjects, Vector3.zero, out RaycastHit hit)) {
            Debug.Log("there's already an object too close to " + spawnPosRnd);
            return; // placeholder
        }

        float xRotRnd = Random.Range(0, 360);
        float yRotRnd = Random.Range(0, 360);
        float zRotRnd = Random.Range(0, 360);

        Instantiate(environmentPrefabs[prefabRnd], spawnPosRnd, Quaternion.Euler(xRotRnd, yRotRnd, zRotRnd));


    }

}
