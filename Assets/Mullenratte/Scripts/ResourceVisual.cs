using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceVisual : MonoBehaviour
{
    public TextMeshProUGUI text;

    private void Start() {
        text.text = transform.root.name;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rot = new Vector3(30, 100, 30) * Time.deltaTime;
        transform.Rotate(rot);
    }
}
