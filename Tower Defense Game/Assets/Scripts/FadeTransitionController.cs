using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTransitionController : MonoBehaviour
{
    private UpgradeUI upgradeUI;
    private GameOverCinematic gameOverCinematic;

    void Start() {
        upgradeUI = UpgradeUI.instance;
        gameOverCinematic = GameOverCinematic.instance;
    }

    void DuringGameOverCinematicFade() {
        gameOverCinematic.StartCoroutine("GameOverCinematics");
    }

    void DuringEnterUpgradeUI() {
        upgradeUI.UIEnable();
    }

    void DuringExitUpgradeUI() {
        upgradeUI.ExitUpgradeUI();
    }
}
