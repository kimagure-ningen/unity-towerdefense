using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverCinematic : MonoBehaviour
{
    public static GameOverCinematic instance;
    void Awake() {
        if (instance != null) {
            Debug.LogError("More than one GameOverCinematic in scene!");
            return;
        }
        instance = this;
    }

    private int transitionCount = 0;
    private SceneMaster sceneMaster;

    void Start() {
        sceneMaster = SceneMaster.instance;
    }

    IEnumerator GameOverCinematics() {
        transitionCount++;
        Debug.Log("Fading!");

        //* Cinematic Scene 1
        if (transitionCount == 1) {
            sceneMaster.statsUI.enabled = false;
            sceneMaster.cinematicBarUI.SetActive(true);
            CinemachineShake.Instance.ChangeFOV(80);
            CinemachineShake.Instance.ShakeCamera(10f,8f);
            sceneMaster.mainCameraController.transform.rotation = Quaternion.identity;
            yield return new WaitForSeconds(8);
            sceneMaster.fadeTransition.SetTrigger("StartGameOverCinematicFade");
        }

        //* Cinematic Scene 2
        if (transitionCount == 2) {
            sceneMaster.mainCameraController.transform.position = new Vector3(0f, 50f, 204f);
            CinemachineShake.Instance.ChangeFOV(20);
            yield return new WaitForSeconds(.5f);
            sceneMaster.mainCameraController.transform.Rotate(0f, .5f, 0f);
            yield return new WaitForSeconds(.01f);
            while (sceneMaster.mainCameraController.transform.rotation.y > 0f) {
                sceneMaster.mainCameraController.transform.Rotate(0f, .5f, 0f);
                yield return new WaitForSeconds(.01f);
            }
            sceneMaster.fadeTransition.SetTrigger("StartGameOverCinematicFade");
        }

        //* Cinematic Scene 3
        if (transitionCount == 3) {
            sceneMaster.mainCameraController.transform.rotation = Quaternion.identity;
            sceneMaster.mainCameraController.transform.position = new Vector3(150f, 70f, 0f);
            sceneMaster.mainCameraController.transform.Rotate(0f, 0, -35f);
            CinemachineShake.Instance.ChangeFOV(60);
            while (sceneMaster.UFO.transform.position.y < 125f) {
                sceneMaster.UFO.transform.Translate(0f, 0f, .25f);
                yield return new WaitForSeconds(.0025f);
            }
            while (sceneMaster.UFO.transform.position.z > 0f) {
                sceneMaster.UFO.transform.Translate(0f, .5f, 0f);
                yield return new WaitForSeconds(.0025f);
            }
            yield return new WaitForSeconds(2);
            Destroy(sceneMaster.Map);
            yield return new WaitForSeconds(1);
            sceneMaster.fadeTransition.SetTrigger("StartGameOver");
            sceneMaster.enabled = false;
        }
    }
}
