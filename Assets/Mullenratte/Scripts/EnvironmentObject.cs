using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentObject : MonoBehaviour
{


    private void Update() {
        if (Mathf.Abs(transform.position.x - Platform.instance.transform.position.x) > ProceduralGenerator.instance.envObjDespawnDistance) {
            ProceduralGenerator.instance.DespawnObjAndAddToPool(this.gameObject, ProceduralGenerator.instance.environmentObjPool);
            Debug.Log("despawned " + this.name);
        }
    }
}
