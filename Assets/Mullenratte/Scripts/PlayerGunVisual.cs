using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunVisual : MonoBehaviour
{
    private PlayerGun gun;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private TrailRenderer bulletTrail;
    [SerializeField] private ParticleSystem OnShootPartSys;
    [SerializeField] private ParticleSystem OnHitPartSys;

    private Vector3 hitPosition;

    private void Awake() {
        gun = GetComponent<PlayerGun>();    
    }

    private void Start() {
        gun.OnShoot += Gun_OnShoot;
        gun.OnHit += Gun_OnHit;
    }

    private void Gun_OnHit(object sender, PlayerGun.OnHitEventArgs e) {
        Debug.Log("hit particle sys");

        OnHitPartSys.Play();
        TrailRenderer trail = Instantiate(bulletTrail, bulletSpawnPoint.position, Quaternion.identity);
        hitPosition = e.hit.point;
        StartCoroutine(CreateTrail(trail, e.hit));
    }

    private void Gun_OnShoot() {
        Debug.Log("shot gun part sys");
        OnShootPartSys.Play();
    }

    IEnumerator CreateTrail(TrailRenderer trail, RaycastHit hit) {
        float progress = 0f;
        Vector3 startPos = trail.transform.position;

        while (progress < 1f) {
            trail.transform.position = Vector3.Lerp(startPos, hitPosition, progress);
            progress += Time.deltaTime / trail.time;

            yield return null;
        }

        trail.transform.position = hit.point;
        Instantiate(OnHitPartSys, hit.point, Quaternion.LookRotation(hit.normal));

        Destroy(trail.gameObject, trail.time);
    }
}
