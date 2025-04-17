using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemiesPrefabs;
    public static GameObject enemySpawned = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesPrefabs != null && enemySpawned == null)
        {
            enemySpawned = Instantiate(enemiesPrefabs[Random.Range(0,3)]);
        }
        
        if(enemySpawned !=null && enemySpawned.transform.localPosition.x < -9)
        {
            Destroy(enemySpawned);
            enemySpawned = null;
        }
        
    }
}
