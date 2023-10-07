using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 5f; 

    void Start()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.down * 9.8f, ForceMode.Acceleration);

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }


    void OnCollisionEnter(Collision col)
    {
        Destroy(gameObject);
    }
}