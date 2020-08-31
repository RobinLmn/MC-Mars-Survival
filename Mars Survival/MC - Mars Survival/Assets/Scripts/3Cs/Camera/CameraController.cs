using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float mouseXSensitivity = 10f;
    [SerializeField] private float mouseYSensitivity = 5f;

    [SerializeField] private Transform myPlayerTransform;

    private float myRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseXSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseYSensitivity;

        myRotation -= mouseY;
        myRotation = Mathf.Clamp(myRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(myRotation, 0f, 0f);
        myPlayerTransform.Rotate(Vector3.up * mouseX);
    }
}
