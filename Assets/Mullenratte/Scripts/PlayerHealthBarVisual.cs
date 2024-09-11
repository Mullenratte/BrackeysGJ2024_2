using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarVisual : MonoBehaviour
{
    [SerializeField] private HealthSystem playerHealthSystem;
    [SerializeField] private Image healthBarImage;

    private void Start() {
        playerHealthSystem.OnDamaged += PlayerHealthSystem_OnDamaged;
        UpdateHealthBar();
    }

    private void PlayerHealthSystem_OnDamaged(object sender, HealthSystem.OnDamagedEventArgs e) {
        UpdateHealthBar();
    }

    private void UpdateHealthBar() {
        healthBarImage.fillAmount = playerHealthSystem.GetHealthNormalized();
    }
}
