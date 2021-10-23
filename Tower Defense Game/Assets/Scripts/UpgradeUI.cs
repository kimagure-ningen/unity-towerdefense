using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    public static UpgradeUI instance;

    void Awake() {
        if (instance != null) {
            Debug.LogError("More than one UpgradeUI in scene!");
            return;
        }
        instance = this;
    }

    private SceneMaster sceneMaster;

    private GameObject selectedTower;
    private Towers selectedTowerScript;

    private bool isEnabled = false;

    [Header("UI")]
    public GameObject strengthUI;
    public GameObject strengthCost;
    public GameObject skillUI;
    public GameObject skillCost;
    public GameObject strengthUpgradeButton;
    public GameObject strengthUpgradeButton2;
    public GameObject skillUpgradeButton;
    public GameObject skillUpgradeButton2;

    public Text strengthCurrentLevel;
    public Text strengthMaximumLevel;
    public Text skillCurrentLevel;
    public Text skillMaximumLevel;

    public Text strengthUpgradeCost;
    public Text skillUpgradeCost;

    void Start() {
        sceneMaster = SceneMaster.instance;

        gameObject.SetActive(false);
        strengthCost.SetActive(false);
        skillCost.SetActive(false);
    }

    void Update() {
        if (isEnabled) {
            transform.Rotate(0f, -10f * Time.deltaTime, 0f);
        }
    }

    void OnEnable() {
        StartCoroutine("StrengthUISwitch");
        StartCoroutine("SkillUISwitch");

        strengthCurrentLevel.text = "Lv." + selectedTowerScript.upgrades.currentStrengthLevel.ToString();
        strengthMaximumLevel.text = "/" + selectedTowerScript.upgrades.maximumStrengthLevel.ToString();
        skillCurrentLevel.text = "Lv." + selectedTowerScript.upgrades.currentSkillLevel.ToString();
        skillMaximumLevel.text = "/" + selectedTowerScript.upgrades.maximumSkillLevel.ToString();

        selectedTowerScript.upgrades.upgradeStrengthCost = selectedTowerScript.upgrades.upgradeStrengthCost * selectedTowerScript.upgrades.currentStrengthLevel;
        strengthUpgradeCost.text = selectedTowerScript.upgrades.upgradeStrengthCost.ToString();
        selectedTowerScript.upgrades.upgradeSkillCost = selectedTowerScript.upgrades.upgradeSkillCost * selectedTowerScript.upgrades.currentSkillLevel;
        skillUpgradeCost.text = selectedTowerScript.upgrades.upgradeSkillCost.ToString();

        strengthUpgradeButton.SetActive(true);
        strengthUpgradeButton2.SetActive(true);
        skillUpgradeButton.SetActive(true);
        skillUpgradeButton2.SetActive(true);
    }

    IEnumerator StrengthUISwitch() {
        while(gameObject.activeSelf) {
            if (selectedTowerScript.upgrades.currentStrengthLevel == selectedTowerScript.upgrades.maximumStrengthLevel){
                strengthUpgradeButton.SetActive(false);
                strengthUpgradeButton2.SetActive(false);
                strengthUI.SetActive(true);
                strengthCost.SetActive(false);
                break;
            }
            strengthUI.SetActive(true);
            strengthCost.SetActive(false);
            yield return new WaitForSeconds(2);
            if (selectedTowerScript.upgrades.currentStrengthLevel == selectedTowerScript.upgrades.maximumStrengthLevel)
                break;
            strengthUI.SetActive(false);
            strengthCost.SetActive(true);
            yield return new WaitForSeconds(2);
        }
    }

    IEnumerator SkillUISwitch() {
        while(gameObject.activeSelf) {
            if (selectedTowerScript.upgrades.currentSkillLevel == selectedTowerScript.upgrades.maximumSkillLevel) {
                skillUpgradeButton.SetActive(false);
                skillUpgradeButton2.SetActive(false);
                skillUI.SetActive(true);
                skillCost.SetActive(false);
                break;
            }
            skillUI.SetActive(true);
            skillCost.SetActive(false);
            yield return new WaitForSeconds(2);
            if (selectedTowerScript.upgrades.currentSkillLevel == selectedTowerScript.upgrades.maximumSkillLevel)
                break;
            skillUI.SetActive(false);
            skillCost.SetActive(true);
            yield return new WaitForSeconds(2);
        }
    }

    public void GetTower(GameObject tower, Towers towerScript) {
        selectedTower = tower;
        selectedTowerScript = towerScript;
    }

    public void UIEnable() {
        sceneMaster.mainCameraController.SetActive(false);
        gameObject.SetActive(true);
        sceneMaster.shopUI.SetActive(false);
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.transform.position = new Vector3(selectedTower.transform.position.x, selectedTower.transform.position.y, selectedTower.transform.position.z);
        isEnabled = true;
    }

    public void OnStrengthUpgradeButton() {
        if (PlayerStats.Money < selectedTowerScript.upgrades.upgradeStrengthCost)
            return;
        
        selectedTowerScript.upgrades.currentStrengthLevel++;
        if (!selectedTowerScript.useLazer) {
            selectedTowerScript.fireRate = selectedTowerScript.fireRate * selectedTowerScript.upgrades.currentStrengthLevel * .6f;
        } else {
            selectedTowerScript.damageOverTime = selectedTowerScript.damageOverTime + selectedTowerScript.upgrades.currentStrengthLevel * 5;
        }
        strengthCurrentLevel.text = "Lv." + selectedTowerScript.upgrades.currentStrengthLevel.ToString();
        PlayerStats.Money -= selectedTowerScript.upgrades.upgradeStrengthCost;
        Debug.Log("Strength Upgraded!");

        selectedTowerScript.upgrades.upgradeStrengthCost = selectedTowerScript.upgrades.upgradeStrengthCost * selectedTowerScript.upgrades.currentStrengthLevel;
        strengthUpgradeCost.text = selectedTowerScript.upgrades.upgradeStrengthCost.ToString();

        if (selectedTowerScript.upgrades.currentStrengthLevel == selectedTowerScript.upgrades.maximumStrengthLevel){
            strengthUpgradeButton.SetActive(false);
            strengthUpgradeButton2.SetActive(false);
            strengthUI.SetActive(true);
            strengthCost.SetActive(false);
        }
    }

    public void OnSkillUpgradeButton() {
        if (PlayerStats.Money < selectedTowerScript.upgrades.upgradeSkillCost)
            return;
        
        selectedTowerScript.upgrades.currentSkillLevel++;
        if (!selectedTowerScript.useLazer) {
           selectedTowerScript.range = selectedTowerScript.range * selectedTowerScript.upgrades.currentStrengthLevel * .6f;
        } else {
            selectedTowerScript.slowPct = selectedTowerScript.slowPct * selectedTowerScript.upgrades.currentStrengthLevel * .6f;
        }
        skillCurrentLevel.text = "Lv." + selectedTowerScript.upgrades.currentSkillLevel.ToString();
        PlayerStats.Money -= selectedTowerScript.upgrades.upgradeSkillCost;
        Debug.Log("Skill Upgraded!");

        selectedTowerScript.upgrades.upgradeSkillCost = selectedTowerScript.upgrades.upgradeSkillCost * selectedTowerScript.upgrades.currentSkillLevel;
        skillUpgradeCost.text = selectedTowerScript.upgrades.upgradeSkillCost.ToString();

        if (selectedTowerScript.upgrades.currentSkillLevel == selectedTowerScript.upgrades.maximumSkillLevel){
            skillUpgradeButton.SetActive(false);
            skillUpgradeButton2.SetActive(false);
            skillUI.SetActive(true);
            skillCost.SetActive(false);
        }
    }

    public void OnDeleteButton() {
        Destroy(selectedTower);
        sceneMaster.fadeTransition.SetTrigger("StartExitUpgradeUI");
    }

    public void OnCancelButton() {
        sceneMaster.fadeTransition.SetTrigger("StartExitUpgradeUI");
    }

    public void ExitUpgradeUI() {
        selectedTower = null;
        gameObject.SetActive(false);
        sceneMaster.mainCameraController.transform.rotation = Quaternion.identity;
        sceneMaster.mainCameraController.SetActive(true);
        sceneMaster.shopUI.SetActive(true);
    }
}
