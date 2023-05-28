using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    PlacementManager placementManager;

    public TowerBlueprint archerTower;
    public TowerBlueprint missileTower;
    public TowerBlueprint lazerTower;

    void Start() {
        placementManager = PlacementManager.instance;
    }

    public void SelectArcherTower() {
        placementManager.SelectTowerToBuild(archerTower);
        Debug.Log("Archer Tower Selected!");
    }

    public void SelectMissileTower() {
        placementManager.SelectTowerToBuild(missileTower);
        Debug.Log("Missile Tower Selected!");
    }

    public void SelectLazerTower() {
        placementManager.SelectTowerToBuild(lazerTower);
        Debug.Log("Lazer Tower Selected!");
    }
}
