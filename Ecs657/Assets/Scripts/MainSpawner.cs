using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MainSpawner : MonoBehaviour
{
    [SerializeField] private List<EntityToSpawn> EntityList;
    [SerializeField] private float maxRadius;
    [SerializeField] private float minRadius;
    [SerializeField] private float offsetY;
    private LayerMask ground;

	#region UnityFunctions
	void Start()
    {
        ground = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        SpawnAllEnemies();
    }

#if UNITY_EDITOR

    void OnDrawGizmosSelected()
    {
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position + Vector3.down * offsetY, new Vector3(0, 1, 0), maxRadius);
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position + Vector3.down * offsetY, new Vector3(0, 1, 0), minRadius);
    }
#endif
#endregion

    #region SpawningEnemies
    //spawns any possible enemy, given their timer
    private void SpawnAllEnemies()
	{
        foreach (EntityToSpawn currentEnemy in EntityList)
		{
            currentEnemy.currentTimer += Time.deltaTime;
            if (currentEnemy.currentTimer < currentEnemy.timeBetweenSpawns)
			{
                continue;
			}
                
            currentEnemy.currentTimer -= currentEnemy.timeBetweenSpawns;
            for(int i = 0; i < currentEnemy.numberPerSpawn; i++)
			{
                SpawnEnemy(currentEnemy);
			}
        }
	}

    private void SpawnEnemy(EntityToSpawn currentEnemy)
	{
        Quaternion rotation = new Quaternion();
        //get random point within the ranges of 2 circles
        RaycastHit hit;
        Vector3 offset = new Vector3(0, 0, 0);
        int repeats = 0;
        do
        {
            repeats++;
            float boundingRadius = Random.Range(minRadius, maxRadius);
            offset = Random.insideUnitCircle.normalized * boundingRadius;
            offset = new Vector3(offset.x + transform.position.x,
                                 transform.position.y - offsetY,
                                 offset.y + transform.position.z);

            //find distance from terrain to spawn entity at correct Y coordinates
            Physics.Raycast(offset, Vector3.down, out hit, 100f, ground);

            //for debugging
            Debug.DrawRay(offset, Vector3.down * 100f);

            offset += new Vector3(0, -hit.distance, 0);
        } while (hit.distance == 0 && repeats < 100); // repeat check for failSafe precaution


        //Make enemy look at player upon spawning
        //float dx = offset.x - transform.position.x;
        //float dy = offset.z - transform.position.z;
        //float directionangle = Mathf.Atan2(dy, dx); // in radians
        //float degrees = directionangle * 180 / Mathf.PI;
        //rotation = Quaternion.Euler(0, degrees, 0);

        GameObject spawnedEnemey = Instantiate(currentEnemy.enemyPrefab, offset, rotation);
        spawnedEnemey.transform.LookAt(transform.position);
        
    }
	#endregion

    //Enemy variables in order to spawn them How I want them too
    [System.Serializable]
    public class EntityToSpawn
	{
        public GameObject enemyPrefab;
        public int numberPerSpawn;
        public float timeBetweenSpawns;
        public float currentTimer;

        public EntityToSpawn(GameObject enemyPrefab, int numberPerSpawn, float timeBetweenSpawns)
		{
            this.enemyPrefab = enemyPrefab;
            this.numberPerSpawn = numberPerSpawn;
            this.timeBetweenSpawns = timeBetweenSpawns;
            currentTimer = 0;
		}
	}
}
