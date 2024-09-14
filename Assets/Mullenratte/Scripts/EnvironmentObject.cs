using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentObject : MonoBehaviour
{


    private void Update() {
        if (transform.position.x < Platform.instance.objPoolTriggerPoint.position.x) {
            ProceduralGenerator.instance.DespawnObjAndAddToPool(this.gameObject, ProceduralGenerator.instance.environmentObjPool);
            Debug.Log("despawned " + this.name);
        }
    }
}
