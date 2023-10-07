using UnityEngine;

public class WaterSpotTrigger : MonoBehaviour
{
    public ParticleSystem waterSprayEffect;

    private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Player"))
    {
        Debug.Log("Player entered the trigger");
        waterSprayEffect.Play();
    }
}
}
