using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingAI : MonoBehaviour
{
    public float speed = 3.0f;
    public float obstacleRange = 5.0f;
    public float rotationTime = 1f; 

    private bool isAlive;
    private bool isRotating = false;
    private float originalSpeed;

    [SerializeField] GameObject fireballPrefab;
    private GameObject fireball;

    private void Start()
    {
        isAlive = true;
        originalSpeed = speed; 
    }

    void Update()
    {
        if (isAlive && !isRotating)
        {
            transform.Translate(0, 0, speed * Time.deltaTime);
        }

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (!isRotating && Physics.SphereCast(ray, 0.75f, out hit))
        {
            GameObject hitObject = hit.transform.gameObject;
            if (hitObject.GetComponent<Player>())
            {
                if (fireball == null)
                {
                    fireball = Instantiate(fireballPrefab) as GameObject;
                    fireball.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
                    fireball.transform.rotation = transform.rotation;
                }
            }
            else if (hit.distance < obstacleRange)
            {
                float angle = Random.Range(-110, 110);
                StartCoroutine(RotateOverTime(angle));
            }
        }
    }

    IEnumerator RotateOverTime(float angle)
    {
        isRotating = true;
        speed = 0; 

        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0, angle, 0);
        float elapsedTime = 0f;

        while (elapsedTime < rotationTime)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / rotationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation;
        speed = originalSpeed; 
        isRotating = false;
    }

    public void SetAlive(bool alive)
    {
        isAlive = alive;
    }
}
