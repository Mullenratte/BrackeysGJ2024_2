using System;
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
    [SerializeField, Range(1f, 5f)] float maxRandomScaleFactor;
    [Header("Enemies")]
    //[SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] Transform enemySpawnerLeft;
    [SerializeField] Transform enemySpawnerRight;
    [Header("Resources")]
    [SerializeField] GameObject[] resourcePrefabs;

    public event EventHandler<OnGenerationPointShiftedEventArgs> OnGenerationPointShifted;

    public class OnGenerationPointShiftedEventArgs : EventArgs {
        public Vector3 oldPos, newPos;
    }

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
            Vector3 oldPos = generationPoint.position;
            generationPoint.position += Vector3.right * generationPointDistanceX;
            Vector3 newPos = generationPoint.position;
            OnGenerationPointShifted?.Invoke(this, new OnGenerationPointShiftedEventArgs { oldPos = oldPos, newPos = newPos});
        }
    }

    private void GenerateNewSection(float spawnPosXOffset) {
        int rnd = UnityEngine.Random.Range(minEnvironmentObjects, maxEnvironmentObjects + 1);
        for (int i = 0; i < rnd; i++) {
            GenerateEnvironment(spawnPosXOffset);
        }

    }

    private void GenerateEnvironment(float spawnPosXOffset) {
        int prefabRnd = UnityEngine.Random.Range(0, environmentPrefabs.Length);

        float distanceOffsetRnd = UnityEngine.Random.Range(minSpawnRadius, maxSpawnRadius);
        /* get a random position on a sphere around the platform center
         * within a random radius between the min and max allowed spawn radius.
         * Then, it is also offset on the X-axis by the maximum spawn radius (because there's no real need to spawn objects behind the platform)
         * and on the Z-axis by 10f (currently a magic number; just a good enough distance from the platform to either side),
         * so there won't be any objects spawning in the platforms path
        */
        float spawnPosZOffset = 10f;
        Vector3 unitSpherePosRnd = UnityEngine.Random.onUnitSphere * distanceOffsetRnd;
        if (unitSpherePosRnd.z < 0) {
            spawnPosZOffset = -MathF.Abs(spawnPosZOffset);
        } else {
            spawnPosZOffset = MathF.Abs(spawnPosZOffset);
        }
        Vector3 spawnPosRnd = unitSpherePosRnd + Platform.instance.procGenTriggerPoint.position + Vector3.right * spawnPosXOffset + Vector3.forward * spawnPosZOffset;   

        if (Physics.SphereCast(spawnPosRnd, minDistanceToObjects, Vector3.zero, out RaycastHit hit)) {
            Debug.Log("there's already an object too close to " + spawnPosRnd);
            return; // placeholder
        }

        float xRotRnd = UnityEngine.Random.Range(0, 360);
        float yRotRnd = UnityEngine.Random.Range(0, 360);
        float zRotRnd = UnityEngine.Random.Range(0, 360);

        GameObject prefab = Instantiate(environmentPrefabs[prefabRnd], spawnPosRnd, Quaternion.Euler(xRotRnd, yRotRnd, zRotRnd));

        float scaleRnd = UnityEngine.Random.Range(1f, maxRandomScaleFactor);
        prefab.transform.localScale *= scaleRnd;


    }

}
