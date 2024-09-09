using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] HealthSystem healthSystem;
    [SerializeField] int damage;

    private void Start() {

        healthSystem.OnDeath += HealthSystem_OnDeath;


    }

    private void HealthSystem_OnDeath(object sender, System.EventArgs e) {
        Destroy(gameObject);    // subject to change
    }

    private void OnCollisionEnter(Collision collision) {
        Debug.Log(collision.gameObject);


        if (collision.collider.TryGetComponent<PlayerMovement>(out _) && collision.collider.TryGetComponent(out HealthSystem playerHealthSystem)) {
            Debug.Log("hit palyer");
            playerHealthSystem.Damage(damage);
        }
    }
}
