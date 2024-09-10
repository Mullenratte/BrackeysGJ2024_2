using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherSystem : MonoBehaviour
{
    public static TetherSystem instance;
    
    public Transform tetherPoint;
    public float maxRange;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }


}
