using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneMaster : MonoBehaviour
{
    public static SceneMaster instance;

    private bool gameEnded = false;

    [Header("Properties")]
    public GameObject waypoint;
    public static Transform[] waypoints;

    public Wave[] waves;

    public Transform spawnPoint;
    
    public static int enemiesAlive = 0;
    public float timeBetweenWaves = 5f;
    private float countdown = 2f;

    private int waveNumber = 0;

    public GameObject mainCameraController;
    public GameObject UFO;
    public GameObject Map;

    [Header("UI")]
    public Text waveCountdownText;
    public Text moneyText;
    
    public Canvas statsUI;
    public GameObject shopUI;
    public GameObject cinematicBarUI;

    public Animator fadeTransition;

    public Slider gasSlider;
    public Gradient gradient;
    public Image sliderFill;

    void Start() {
        cinematicBarUI.SetActive(false);
    }

    void Awake() {
        instance = this;

        waypoints = new Transform[waypoint.transform.childCount];
        for (int i = 0; i < waypoints.Length; i++) {
            waypoints[i] = waypoint.transform.GetChild(i);
        }
    }

    void Update() {
        if (gameEnded)
            return;
        
        Debug.Log(PlayerStats.Money);
        moneyText.text = PlayerStats.Money.ToString();

        gasSlider.value = PlayerStats.Gas;
        sliderFill.color = gradient.Evaluate(gasSlider.normalizedValue);

        if (enemiesAlive > 0)
            return;
        
        if (countdown <= 0f) {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
            return;
        }

        if (PlayerStats.Gas >= 100) {
            EndGame();
        }

        countdown -= Time.deltaTime;

        waveCountdownText.text = Mathf.Round(countdown).ToString();
    }

    IEnumerator SpawnWave() {
        Debug.Log("Spawning Waves!");
        Debug.Log("Wave " + waveNumber);

        Wave wave = waves[waveNumber];

        for (int i = 0; i < wave.count; i++) {
            SpawnEnemy(wave.enemy);
            yield return new WaitForSeconds(1f / wave.rate);
        }
        waveNumber++;

        if (waveNumber == waves.Length) {
            WinGame();
        }
    }

    void SpawnEnemy(GameObject enemy) {
        Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
        enemiesAlive++;
    }

    void EndGame() {
        gameEnded = true;
        Debug.Log("Game Over!");
        fadeTransition.SetTrigger("StartGameOverCinematicFade");
    }

    void WinGame() {
        Debug.Log("You Won!");
        fadeTransition.SetTrigger("StartGameClear");
        this.enabled = false;
    }
}