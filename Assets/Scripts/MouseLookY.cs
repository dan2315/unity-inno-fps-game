using UnityEngine;

public class MouseLookY : MonoBehaviour
{
    public float sensitivity = 100f;
    private float xRotation = 0f;

    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}