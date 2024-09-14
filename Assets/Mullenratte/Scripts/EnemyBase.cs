using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("DEBUG")]
    [SerializeField] TextMeshProUGUI text;
    private Rigidbody rb;

    [Header("General")]
    [SerializeField] HealthSystem healthSystem;
    [SerializeField] int damage;
    [SerializeField] float movementSpeed;
    [SerializeField] float maxVelocity;


    [Header("Roam State")]
    [SerializeField] Transform roamingPoint;
    [SerializeField] float maxRoamDuration;
    [SerializeField] float maxRoamDistance, minRoamDistance;
    float roamTimer = 0f;

    [Header("Attack State")]
    [SerializeField] float minAttackRange;
    [SerializeField] float attackBuildupTime;
    [SerializeField] float attackCooldown;
    [SerializeField] float attackMovementSpeedMultiplier;
    [SerializeField] float attackPrepareMaxVelocity;
    Vector3 lastSeenTargetPosition;
    bool attackFinished, isCharging;
    float attackTimer;

    [Header("Flee State")]
    [SerializeField] float fleeDamageThreshold;
    float totalFleeDamage = 0f;
    [SerializeField] float maxFleeDuration;
    [SerializeField] float fleeVelocityMultiplier;
    [SerializeField] float fleeMovementSpeedMultiplier;
    float fleeTimer = 0f;

    [Header("Drops")]
    [SerializeField] ResourceBase[] droppableResources;
    [SerializeField] int minDroppedResources;
    [SerializeField] int maxDroppedResources;


    public enum BehaviourState {
        Roaming,
        Attacking,
        Fleeing
    }

    private BehaviourState state;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void Start() {
        roamingPoint = Platform.instance.transform;
        state = BehaviourState.Roaming;
        healthSystem.OnDeath += HealthSystem_OnDeath;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;

        attackTimer = 0;
    }

    private void Update() {
        switch (state) {
            case BehaviourState.Roaming:
                //Debug.Log("state - [ROAMING]");
                roamingPoint = Platform.instance.transform;

                roamTimer += Time.deltaTime;
                if (roamTimer > maxRoamDuration) {
                    state = BehaviourState.Attacking;
                }

                break;
            case BehaviourState.Attacking:
                //Debug.Log("state - [ATTACKING]");
                roamTimer = 0f;
                if (totalFleeDamage > fleeDamageThreshold) {
                    totalFleeDamage = 0f;
                    fleeTimer = 0f;
                    state = BehaviourState.Fleeing;
                }
                if (attackFinished) {
                    attackFinished = false;
                    state = BehaviourState.Roaming;
                }
                break;
            case BehaviourState.Fleeing:
                //Debug.Log("state - [FLEEING]");
                roamTimer = 0f;
                fleeTimer += Time.deltaTime;

                if (fleeTimer > maxFleeDuration) {
                    state = BehaviourState.Roaming;
                }
                break;
            default:
                break;
        }

        attackTimer += Time.deltaTime;
    }

    private void FixedUpdate() {
        switch (state) {
            case BehaviourState.Roaming:
                RoamPlatform();
                break;
            case BehaviourState.Attacking:
                GameObject target = roamingPoint.gameObject;
                if (Vector3.Distance(transform.position, PlayerMovement.instance.transform.position) < Vector3.Distance(transform.position, roamingPoint.position)) {
                    target = PlayerMovement.instance.gameObject;
                }
                if (attackTimer >= attackCooldown) AttackTarget(target);
                break;
            case BehaviourState.Fleeing:
                FleePlayer();
                break;
            default:
                break;
        }
    }

    private void HealthSystem_OnDamaged(object sender, HealthSystem.OnDamagedEventArgs e) {
        Debug.Log("Damaged " + gameObject.name);
        totalFleeDamage += e.damageAmount;


    }

    private void HealthSystem_OnDeath(object sender, System.EventArgs e) {
        int dropAmount = Random.Range(minDroppedResources, maxDroppedResources + 1);
        for (int i = 0; i < dropAmount; i++) {
            int rnd = UnityEngine.Random.Range(0, droppableResources.Length);

            Vector3 dropSpawnPos = Random.onUnitSphere;

            Instantiate(droppableResources[rnd], transform.position + dropSpawnPos, Quaternion.identity);
        }

        Destroy(gameObject);    // subject to change
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.TryGetComponent<PlayerMovement>(out _) && collision.collider.TryGetComponent(out HealthSystem playerHealthSystem)) {
            Debug.Log("hit player");
            playerHealthSystem.Damage(damage);
        }
    }

    private void RoamPlatform() {
        //text.text = state.ToString();

        float regulationFactor = 1f;
        Vector3 roamDirection = roamingPoint.root.GetComponent<Rigidbody>().velocity.normalized;

        if (Vector3.Distance(transform.position, roamingPoint.position) > maxRoamDistance) {
            transform.LookAt(roamingPoint);
            roamDirection = (roamingPoint.position - transform.position).normalized;
        }

        if (Vector3.Distance(transform.position, roamingPoint.position) < minRoamDistance) {
            roamDirection = new Vector3(roamDirection.x + Random.Range(-15f, 15f), roamDirection.y + Random.Range(-10f, 10f), roamDirection.z + Random.Range(-20f, 20f)).normalized;
            transform.LookAt(roamDirection);
        }

        if (rb.velocity.magnitude > maxVelocity) {
            regulationFactor = 0.03f;
        }
        rb.AddForce(roamDirection * movementSpeed * regulationFactor);


    }

    private void AttackTarget(GameObject target) {
        //text.text = isCharging ? text.text = "Attacking - Charging" : state.ToString();

        Vector3 attackDirection = (target.transform.position - transform.position).normalized;
        
        if (!isCharging) {
            if (Vector3.Distance(transform.position, target.transform.position) < minAttackRange) {
                rb.AddForce(-rb.velocity);
            } else {
                rb.AddForce(attackDirection * movementSpeed);
            }

            if (rb.velocity.magnitude <= attackPrepareMaxVelocity) {
                isCharging = true;
                StartCoroutine(ChargeUpAttack(attackDirection, attackBuildupTime));
            }
        }
        


    }

    IEnumerator ChargeUpAttack(Vector3 attackDirection, float seconds) {

        yield return new WaitForSeconds(seconds);
        attackDirection = (lastSeenTargetPosition - transform.position).normalized;
        rb.AddForce(attackDirection * movementSpeed * attackMovementSpeedMultiplier, ForceMode.Impulse);
        attackFinished = true;
        isCharging = false;
        attackTimer = 0;
        Debug.Log("attack finished");

    }


    private void FleePlayer() {
        //text.text = state.ToString();

        float regulationFactor = 1f;

        Vector3 fleeDirection = (transform.position - PlayerMovement.instance.transform.position).normalized;

        if (rb.velocity.magnitude > maxVelocity * fleeVelocityMultiplier) {
            regulationFactor = 0.03f;
        }

        rb.AddForce(fleeDirection * movementSpeed * fleeMovementSpeedMultiplier * regulationFactor);

    }
}
