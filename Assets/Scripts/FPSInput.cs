using UnityEngine;

public class FPSInput : MonoBehaviour
{
    public float speed = 5f;
    private CharacterController controller;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        if (Input.GetKey(KeyCode.LeftShift))
            speed = 10f;
        else
            speed = 5f;

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
    }
}
