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
        PlayerInteractSystem.instance.OnNotHover += PlayerInteractSystem_OnNotHover;
        hoverText.text = "";
    }

    private void PlayerInteractSystem_OnNotHover(object sender, PlayerInteractSystem.OnInteractEventArgs e) {
        hoverText.text = "";
    }

    private void PlayerInteractSystem_OnHover(object sender, PlayerInteractSystem.OnInteractEventArgs e) {
        
        hoverText.text = "[E] " + (e.hit.transform.TryGetComponent<ICollectible>(out _) ? "Collect" : "Interact");
    }
}
