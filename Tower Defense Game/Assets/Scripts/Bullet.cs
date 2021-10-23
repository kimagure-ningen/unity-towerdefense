using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;

    public float speed = 70f;

    public float explosionRadius = 0f;

    public int damage = 50;

    public void Seak(Transform _target) {
        target = _target;
    }

    void Update() {
        if (target == null) {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame) {
            HitTarget();
            return;
        }
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }

    void HitTarget() {
        if (explosionRadius > 0f) {
            Explode();
        } else {
            Damage(target);
        }
        Destroy(gameObject);
    }

    void Damage(Transform enemy) {
        Enemy _enemy = enemy.GetComponent<Enemy>();

        if (_enemy != null) {
            _enemy.TakeDamage(damage);
        }
    }

    void Explode() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders) {
            if (collider.tag == "Enemy") {
                Debug.Log("Exploding");
                Damage(collider.transform.parent);
            }
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
