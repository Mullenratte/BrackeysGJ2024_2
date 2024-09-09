using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] HealthSystem healthSystem;

    private void Start() {

        healthSystem.OnDeath += HealthSystem_OnDeath;


    }

    private void HealthSystem_OnDeath(object sender, System.EventArgs e) {
        Destroy(gameObject);    // subject to change
    }
}
