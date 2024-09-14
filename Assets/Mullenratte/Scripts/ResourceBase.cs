using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBase : MonoBehaviour, ICollectible
{

    public enum BuffType {
        PlatformHealth,
        PlayerDamage,
        PlayerVelocity,
        PlayerHealth
    }
    [SerializeField] private BuffType buffType;

    public static event EventHandler<OnCollectEventArgs> OnCollect;

    public class OnCollectEventArgs : EventArgs {
        public BuffType buffType;
    }

    

    public void CollectBehaviour() {
        OnCollect?.Invoke(this, new OnCollectEventArgs { buffType = this.buffType });
        Destroy(gameObject);
    }

    private void Update() {
        if (transform.position.x < Platform.instance.objPoolTriggerPoint.position.x) {
            ProceduralGenerator.instance.DespawnObjAndAddToPool(this.gameObject, ProceduralGenerator.instance.resourceObjPool);
        }
    }

}
