using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlacementManager : MonoBehaviour
{
    public static PlacementManager instance;

    void Awake() {
        if (instance != null) {
            Debug.LogError("More than one PlacementManager in scene!");
            return;
        }
        instance = this;
    }

    private bool isPlacing = false;

    public Mesh buildingPreviewMesh;
    public Material buildingPreviewMat;
    public Material buildingProhibitMat;

    private TowerBlueprint towerToBuild;

    public UpgradeUI upgradeUI;
    
    void Update() {
        if (isPlacing)
        {
            Mesh viewedModelFilter = towerToBuild.previewPrefab.GetComponent<MeshFilter>().sharedMesh;
            buildingPreviewMesh = viewedModelFilter;

            Vector3 position = Vector3.zero;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit info_b, 1000f, LayerMask.GetMask("Obstacle"))) {
                position = info_b.point;
                //Debug.Log(info_b.point);
                MouseToBasePosition(position);
                return;
            }

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit info_c, 1000f, LayerMask.GetMask("Tower"))) {
                position = info_c.point;
                //Debug.Log(info_c.point);
                MouseToBasePosition(position);
                return;
            }

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit info_a, 1000f, LayerMask.GetMask("SetSurface"))) {
                position = info_a.point;
                //Debug.Log(info_a.point);

                MouseToPlacementPosition(position);
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (PlayerStats.Money < towerToBuild.cost)
                    return;
                if (position != Vector3.zero) {
                    SpawnBuilding(position);
                    PlayerStats.Money -= towerToBuild.cost;
                }
                isPlacing = false;
                position = Vector3.zero;
            }
        }
    }

    public void TowerSelected(GameObject tower, Towers towerScript) {
        Debug.Log(tower + " Selected!");
        upgradeUI.GetTower(tower, towerScript);
        SceneMaster.instance.fadeTransition.SetTrigger("StartEnterUpgradeUI");
    }

    public void SelectTowerToBuild(TowerBlueprint tower) {
        towerToBuild = tower;
        isPlacing = true;
    }

    void SpawnBuilding(Vector3 position)
    {
        // ** Create Building
        Instantiate(towerToBuild.prefab, position, Quaternion.identity);
    }

    void MouseToPlacementPosition(Vector3 position) {
        if (position != Vector3.zero) {
            Graphics.DrawMesh(buildingPreviewMesh, position, Quaternion.identity, buildingPreviewMat, 0);
        }
    }
    
    void MouseToBasePosition(Vector3 position) {
        if (position != Vector3.zero) {
            Graphics.DrawMesh(buildingPreviewMesh, position, Quaternion.identity, buildingProhibitMat, 0);
        }
    }
}
