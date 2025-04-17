using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float minSpace;
    public float maxSpace;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * GameManager.velocity * Time.deltaTime);
    }
}
