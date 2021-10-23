using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    void Update() {
        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(0f, 20f * Time.deltaTime, 0f);
        }
        if (Input.GetKey(KeyCode.D)) {
            transform.Rotate(0f, -20f * Time.deltaTime, 0f);
        }
    }
}
