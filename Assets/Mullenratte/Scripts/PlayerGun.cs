using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    public LayerMask platformLayer;

    [SerializeField] float fireCooldown;
    float shootTimer;
    [SerializeField] float gunRange;
    [SerializeField] int gunDamage;

    [SerializeField] Transform playerCamera;

    public event Action OnShoot;
    public event EventHandler<OnHitEventArgs> OnHit;

    public class OnHitEventArgs : EventArgs {
        public RaycastHit hit;
        public IShootable shotObj;
    }

    private void Update() {

        shootTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            TryShoot();
        }
    }


    private void TryShoot() {
        if (shootTimer < 0) {
            OnShoot?.Invoke();
            shootTimer = fireCooldown;
            if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, gunRange, ~platformLayer)) {
                if(hit.collider.gameObject.TryGetComponent(out IShootable shotObject)) {
                    Debug.Log("dealt " + gunDamage + " damage to " + shotObject);
                    shotObject.Damage(gunDamage);
                    OnHit?.Invoke(this, new OnHitEventArgs { shotObj = shotObject, hit = hit });
                }
            }

        }
    }

    public void ChangeGunDamage(int amount) {
        gunDamage += amount;
    }

    public int GetGunDamage() {
        return gunDamage;
    }
}
