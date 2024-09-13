using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBase : MonoBehaviour, ICollectible
{
    public void CollectBehaviour() {
        Debug.Log("Collected " + name);
        Destroy(gameObject);
    }

}
