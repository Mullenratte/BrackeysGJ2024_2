using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public static Platform instance;
    
    public Transform tetherPoint;
    public Transform procGenTriggerPoint;
    public Transform objPoolTriggerPoint;
    public float maxRange;
    public Vector3 PreviousPosition { get; private set; }
    public Vector3 Velocity { get; private set; }

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        PreviousPosition = transform.position;
    }

    private void Update() {
        Velocity = (transform.position - PreviousPosition) / Time.deltaTime;
        PreviousPosition = transform.position;
    }




}
