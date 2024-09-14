using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBase : MonoBehaviour, ICollectible
{
    public void CollectBehaviour() {
        Destroy(gameObject);
    }

    private void Update() {
        if (transform.position.x < Platform.instance.objPoolTriggerPoint.position.x) {
            ProceduralGenerator.instance.DespawnObjAndAddToPool(this.gameObject, ProceduralGenerator.instance.resourceObjPool);
        }
    }

}
