using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hoverText;

    // Start is called before the first frame update
    void Start()
    {
        PlayerInteractSystem.instance.OnHover += PlayerInteractSystem_OnHover;
    }

    private void PlayerInteractSystem_OnHover(object sender, PlayerInteractSystem.OnInteractEventArgs e) {
        hoverText.text = "[E] " + (e.hit.transform.TryGetComponent<ICollectible>(out _) ? "Collect" : "Interact");
    }

    // Update is called once per frame
    void Update()
    {
        hoverText.text = "";

        //if (!PlayerInteractSystem.instance.isHoveringObject) {
        //    hoverText.text = "";
        //}
    }
}
