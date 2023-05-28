using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float startSpeed = 25f;
    [HideInInspector]
    public float speed;

    public float startHealth = 100;
    private float health;

    public int enemyValue = 50;

    public Slider healthSlider;
    public Gradient gradient;
    public Image sliderFill;

    private Transform target;
    private int waypointCounter = 0;

    void Start() {
        target = SceneMaster.waypoints[0];
        speed = startSpeed;
        health = startHealth;
        healthSlider.value = health / startHealth * 100;
    }

    void Update() {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.4f) {
            GetNextWaypoint();
        }

        speed = startSpeed;
    }

    public void TakeDamage(float amount) {
        health -= amount;
        healthSlider.value = health / startHealth * 100;
        sliderFill.color = gradient.Evaluate(healthSlider.normalizedValue);

        if (health <= 0) {
            Die();
        }
    }

    public void Slow(float pct) {
        speed = startSpeed * (1f - pct); 
    }

    void Die() {
        PlayerStats.Money += enemyValue;
        SceneMaster.enemiesAlive--;

        Destroy(gameObject);
    }

    void GetNextWaypoint() {
        if (waypointCounter >= SceneMaster.waypoints.Length - 1) {
            EndPath();
            return;
        }
        waypointCounter++;
        target = SceneMaster.waypoints[waypointCounter];
    }

    void EndPath() {
        PlayerStats.Gas+= 5;
        SceneMaster.enemiesAlive--;

        Destroy(gameObject);
    }
}
