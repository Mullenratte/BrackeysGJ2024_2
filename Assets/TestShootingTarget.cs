using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShootingTarget : MonoBehaviour, IShootable {
    public void TakeDamage(float damage) {
        Debug.Log("Dealt " + damage + " damage!");
    }
}
