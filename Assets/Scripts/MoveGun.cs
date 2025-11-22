using System;
using UnityEngine;

public class MoveGun : MonoBehaviour
{
    private Camera fpsCamera;
    private void Start()
    {
        fpsCamera = GameObject.Find("CameraHolder/Main Camera").GetComponent<Camera>();
    }
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Pew");
        }
    }
}
