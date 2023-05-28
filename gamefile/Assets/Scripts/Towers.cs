using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Towers : MonoBehaviour
{
    private Transform target;
    private Enemy targetEnemy;

    [Header("General")]
    public float range = 45f;
    public TowerUpgradeBlueprint upgrades;

    [Header("For Bullet Tower (default)")]
    public GameObject bulletPrefab;
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    [Header("For Lazer Tower")]
    public bool useLazer = false;
    public int damageOverTime = 30;
    public float slowPct = .5f;

    public LineRenderer lineRenderer;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";

    public float turnSpeed = 10f;

    public Transform firePoint;

    void Start() {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);

        upgrades.currentStrengthLevel = 1;
        upgrades.currentSkillLevel = 1;
    }

    void UpdateTarget() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach(GameObject enemy in enemies) {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance) {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range) {
            target = nearestEnemy.transform;
            targetEnemy = nearestEnemy.GetComponent<Enemy>();
        } else {
            target = null;
        }
    }

    void Update() {
        if (target == null) {
            if (useLazer) {
                if (lineRenderer.enabled) {
                    lineRenderer.enabled = false;
                }
            }
            return;
        }
        
        LockOnTarget();

        if (useLazer) {
            Lazer();
        } else {
            if (fireCountdown <= 0f) {
                Shoot();
                fireCountdown = 1f / fireRate;
            }

            fireCountdown -= Time.deltaTime;
        }
    }

    void LockOnTarget() {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void Lazer() {
        targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);
        targetEnemy.Slow(slowPct);

        if (!lineRenderer.enabled) {
            lineRenderer.enabled = true;
        }
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);
    }

    void Shoot() {
        GameObject bulletGameObj = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGameObj.GetComponent<Bullet>();

        if (bullet != null) {
             bullet.Seak(target);
        }
    }

    void OnMouseDown() {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        PlacementManager.instance.TowerSelected(gameObject, this);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }   
}
