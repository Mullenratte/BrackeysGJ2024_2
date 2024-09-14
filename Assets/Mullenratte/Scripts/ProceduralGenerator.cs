using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
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
    [SerializeField] Transform envObjParentContainer;
    [SerializeField] Transform resObjParentContainer;
    [SerializeField] GameObject[] environmentPrefabs;
    [SerializeField] int minEnvironmentObjects;
    [SerializeField] int maxEnvironmentObjects;
    [SerializeField] float minObjDistance;
    [HideInInspector] public List<GameObject> environmentObjPool;
    public float envObjDespawnDistance;
    const int ENVOBJPOOL_MAXSIZE = 120;
    [SerializeField, Range(1f, 5f)] float maxRandomScaleFactor;
    [Header("Enemies")]
    //[SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] Transform enemySpawnerLeft;
    [SerializeField] Transform enemySpawnerRight;
    [Header("Resources")]
    [SerializeField] GameObject[] resourcePrefabs;
    [SerializeField] int minResourceObjects;
    [SerializeField] int maxResourceObjects;
    [HideInInspector] public List<GameObject> resourceObjPool;
    const int RESOBJPOOL_MAXSIZE = 80;


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
        GenerateEnvironmentObjectPool(ENVOBJPOOL_MAXSIZE);
        GenerateResourceObjectPool(RESOBJPOOL_MAXSIZE);
        GenerateNewSection(-3*maxSpawnRadius);
        GenerateNewSection(-2*maxSpawnRadius);
        GenerateNewSection(-maxSpawnRadius);
        GenerateNewSection(0);
        environmentObjPool = new List<GameObject>();
        resourceObjPool = new List<GameObject>();


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
    private void GenerateEnvironmentObjectPool(int amount) {
        for (int i = 0; i < amount; i++) {
            int prefabRnd = UnityEngine.Random.Range(0, environmentPrefabs.Length);

            GameObject prefab = Instantiate(environmentPrefabs[prefabRnd], Vector3.zero, Quaternion.identity);

            prefab.SetActive(false);

            environmentObjPool.Add(prefab);
            prefab.transform.SetParent(envObjParentContainer);
        }

    }

    private void GenerateResourceObjectPool(int amount) {
        for (int i = 0; i < amount; i++) {
            int prefabRnd = UnityEngine.Random.Range(0, resourcePrefabs.Length);

            GameObject prefab = Instantiate(resourcePrefabs[prefabRnd], Vector3.zero, Quaternion.identity);

            prefab.SetActive(false);

            resourceObjPool.Add(prefab);
            prefab.transform.SetParent(resObjParentContainer);
        }

    }

    private void GenerateNewSection(float spawnPosXOffset) {
        GenerateEnvironment(spawnPosXOffset);

        int resRnd = UnityEngine.Random.Range(minResourceObjects, maxResourceObjects + 1);
        for (int i = 0; i < resRnd; i++) {
            GenerateResources(spawnPosXOffset);
        }
    }

    private void GenerateEnvironment(float spawnPosXOffset) {
        int envRnd = UnityEngine.Random.Range(minEnvironmentObjects, maxEnvironmentObjects + 1);
        for (int i = 0; i < envRnd; i++) {

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

            if (Physics.SphereCast(spawnPosRnd, minObjDistance, Vector3.zero, out RaycastHit hit)) {
                Debug.Log("there's already an object too close to " + spawnPosRnd);
                return; // placeholder
            }

            GameObject spawnedObj = GetRandomObjFromPool(environmentObjPool);

            PlaceObjAtPosWithRandomRotScale(spawnedObj, spawnPosRnd);            
        }
    }



    private void PlaceObjAtPosWithRandomRotScale(GameObject obj, Vector3 position) {
        if (obj == null) return;

        float xRotRnd = UnityEngine.Random.Range(0, 360);
        float yRotRnd = UnityEngine.Random.Range(0, 360);
        float zRotRnd = UnityEngine.Random.Range(0, 360);
        obj.transform.rotation = Quaternion.Euler(xRotRnd, yRotRnd, zRotRnd);

        float scaleRnd = UnityEngine.Random.Range(1f, maxRandomScaleFactor);
        obj.transform.localScale *= scaleRnd;

        obj.transform.position = position;

        obj.SetActive(true);
    }

    private GameObject GetRandomObjFromPool(List<GameObject> pool) {
        if (pool.Count == 0) return null;
        int rnd = UnityEngine.Random.Range(0, pool.Count);
        GameObject obj = pool[rnd];
        pool.Remove(obj);
        return obj;
    }

    public void DespawnObjAndAddToPool(GameObject obj, List<GameObject> pool) {
        pool.Add(obj);
        obj.SetActive(false);
    }

    private void GenerateResources(float spawnPosXOffset) {
        int prefabRnd = UnityEngine.Random.Range(0, resourcePrefabs.Length);

        float maxSpawnDistToTetherPoint = Vector3.Distance(Platform.instance.tetherPoint.position, PlayerMovement.instance.transform.position);
        float distanceOffsetRnd = UnityEngine.Random.Range(minSpawnRadius, maxSpawnDistToTetherPoint);

        Vector3 unitSpherePosRnd = UnityEngine.Random.onUnitSphere * distanceOffsetRnd;

        Vector3 spawnPosRnd = unitSpherePosRnd + Platform.instance.procGenTriggerPoint.position + Vector3.right * spawnPosXOffset;

        if (Physics.SphereCast(spawnPosRnd, minObjDistance, Vector3.zero, out RaycastHit hit)) {
            Debug.Log("there's already an object too close to " + spawnPosRnd);
            return; 
        }

        float xRotRnd = UnityEngine.Random.Range(0, 360);
        float yRotRnd = UnityEngine.Random.Range(0, 360);
        float zRotRnd = UnityEngine.Random.Range(0, 360);

        GameObject prefab = Instantiate(resourcePrefabs[prefabRnd], spawnPosRnd, Quaternion.Euler(xRotRnd, yRotRnd, zRotRnd));

        prefab.transform.SetParent(resObjParentContainer);

    }

}
