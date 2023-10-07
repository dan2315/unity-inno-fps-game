using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public int health = 5;

    public Vector3 position = new Vector3();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void Hurt(int damage) {
		if (health > 0) {
			health -= damage;
			Debug.Log($"Health: {health}");
		}
}



}