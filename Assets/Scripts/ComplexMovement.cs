using UnityEngine;

public class ComplexMovement : MonoBehaviour
{
    public float rotationSpeed = 30f;
    public float movementFrequency = 2f;
    public float movementAmplitude = 2f;
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, rotationSpeed * Time.deltaTime);

        float yOffset = Mathf.Sin(Time.time * movementFrequency) * movementAmplitude;
        transform.position = new Vector3(initialPosition.x, initialPosition.y + yOffset, initialPosition.z);
    }
}