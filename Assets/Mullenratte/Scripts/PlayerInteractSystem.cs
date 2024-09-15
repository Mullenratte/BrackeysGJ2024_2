using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerGun;

public class PlayerInteractSystem : MonoBehaviour
{
    public static PlayerInteractSystem instance;
    [SerializeField] float interactRange;

    public event EventHandler<OnInteractEventArgs> OnHover;
    public event EventHandler<OnInteractEventArgs> OnNotHover;
    public event EventHandler OnInteract;
    public event EventHandler OnCollect;

    public class OnInteractEventArgs : EventArgs {
        public RaycastHit hit;
    }

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Update() {
        if (Physics.Raycast(PlayerCam.instance.transform.position, PlayerCam.instance.transform.forward, out RaycastHit hit, interactRange)) {
            if (hit.collider.gameObject.TryGetComponent(out IInteractable interactObj)) {
                OnHover?.Invoke(this, new OnInteractEventArgs { hit = hit});

                if (Input.GetKeyDown(KeyCode.E)) {
                    OnInteract?.Invoke(this, EventArgs.Empty);
                    if (hit.collider.gameObject.TryGetComponent(out ICollectible collectObj)) {
                        OnCollect?.Invoke(this, EventArgs.Empty);
                        collectObj.CollectBehaviour();
                    }
                }
            } 
        } else {
            OnNotHover?.Invoke(this, new OnInteractEventArgs { hit = hit });
        }
    }
}
