using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] float fireCooldown;
    float shootTimer;
    [SerializeField] float gunRange;
    [SerializeField] float gunDamage;

    [SerializeField] Transform playerCamera;

    private void Update() {

        shootTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            TryShoot();
        }
    }


    private void TryShoot() {
        if (shootTimer < 0) {
            shootTimer = fireCooldown;
            if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, gunRange)) {
                if(hit.collider.gameObject.TryGetComponent(out IShootable shotObject)) {
                    shotObject.TakeDamage(gunDamage);
                }
            }

        }
    }
}
