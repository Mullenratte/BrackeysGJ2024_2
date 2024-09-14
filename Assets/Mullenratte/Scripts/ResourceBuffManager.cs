
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBuffManager : MonoBehaviour
{
    public static ResourceBuffManager instance;

    [SerializeField] PlayerGun playerGun;
    [SerializeField] HealthSystem platformHealth;
    [SerializeField] HealthSystem playerHealth;
    [SerializeField] PlayerMovement playerMovement;

    private float gunDamageBonus;
    private float platformHealthBonus;
    private float platformVelocityBonus;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        ResourceBase.OnCollect += ResourceBase_OnCollect;
    }

    private void ResourceBase_OnCollect(object sender, ResourceBase.OnCollectEventArgs e) {
        switch (e.buffType) {
            case ResourceBase.BuffType.PlatformHealth:
                platformHealth.ChangeHealthMax(5);
                Debug.Log("platform Health: " + platformHealth.GetHealthMax());
                break;
            case ResourceBase.BuffType.PlayerDamage:
                playerGun.ChangeGunDamage(1);
                Debug.Log("gun damage " + playerGun.GetGunDamage());

                break;
            case ResourceBase.BuffType.PlayerVelocity:
                playerMovement.ChangeMaxVelocity(0.5f);

                break;
            case ResourceBase.BuffType.PlayerHealth:
                playerHealth.ChangeHealthMax(3);
                Debug.Log("player health: " + playerHealth.GetHealthMax());

                break;
            default:
                break;
        }
    }
}
