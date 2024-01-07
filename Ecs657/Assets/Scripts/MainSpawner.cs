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
    [SerializeField] private float waveMaxRadius;
    [SerializeField] private float waveMinRadius;

    private LayerMask ground;

	#region UnityFunctions
	void Start()
    {
        ground = LayerMask.GetMask("Ground");
        SpawnAllEnemies();
    }

#if UNITY_EDITOR

    void OnDrawGizmosSelected()
    {
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position + Vector3.down * offsetY, new Vector3(0, 1, 0), maxRadius);
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
            StartSpawningEnemy(currentEnemy);
        }
	}
    
    public void StartSpawningEnemy(EntityToSpawn enemy)
    {
        Coroutine spawnEnemy = StartCoroutine(SpawnEnemy(enemy));
        StartCoroutine(StopSpawning(enemy, spawnEnemy));
    }

    IEnumerator StopSpawning(EntityToSpawn currentEnemy, Coroutine spawnEnemy)
	{
        yield return new WaitForSeconds(currentEnemy.whenToStopSpawning);
        StopCoroutine(spawnEnemy);
	}

    IEnumerator SpawnEnemy(EntityToSpawn currentEnemy)
	{
        yield return new WaitForSeconds(currentEnemy.whenToStartSpawning);

        while (currentEnemy.canSpawn)
		{
            //change spawning settings depending if wave spawn is on or not
            Vector3 position = transform.position;
            float changedMinRadius = minRadius;
            float changedMaxRadius = maxRadius;

            if (currentEnemy.waveSpawn)
            {
                position = GetOffset(transform.position, minRadius, maxRadius);
                changedMinRadius = waveMinRadius;
                changedMaxRadius = waveMaxRadius;
            }

            for (int i = 0; i < currentEnemy.numberPerSpawn; i++)
            {
                Vector3 offset = GetOffset(position, changedMinRadius, changedMaxRadius);

                //Instantiate enemy and make them point towards players x, and z coordinates
                GameObject spawnedEnemy = Instantiate(currentEnemy.enemyPrefab, offset + currentEnemy.offset, Quaternion.identity);
                Vector3 LookPosition = new Vector3(transform.position.x,
                                                    spawnedEnemy.transform.position.y,
                                                    transform.position.z);

                spawnedEnemy.transform.LookAt(LookPosition);
            }

            yield return new WaitForSeconds(currentEnemy.timeBetweenSpawns);
        }
    }
	#endregion

    //returns position within minRadius and maxRadius from a certain position.
    private Vector3 GetOffset(Vector3 position, float minRadius, float maxRadius)
	{
        //get random point within the ranges of 2 circles
        RaycastHit hit;
        Vector3 offset = new Vector3(0, 0, 0);
        int repeats = 0;
        do
        {
            repeats++;
            float boundingRadius = Random.Range(minRadius, maxRadius);
            offset = Random.insideUnitCircle.normalized * boundingRadius;
            offset = new Vector3(offset.x + position.x,
                                 -offsetY + position.y,
                                 offset.y + position.z);

            //find distance from terrain to spawn entity at correct Y coordinates
            Physics.Raycast(offset, Vector3.down, out hit, 100f, ground);

            //for debugging
            Debug.DrawRay(offset, Vector3.down * 100f, Color.white, 10f);

            offset += new Vector3(0, -hit.distance, 0);
        } while (hit.distance == 0 && repeats < 100); // repeat check for failsafe precaution
        return offset; //returns x,y,z position of where the enemy is going to spawn.
    }

    public void addEntity(GameObject enemyPrefab,
                            int numberPerSpawn,
                            float timeBetweenSpawns,
                            float whenToStartSpawning,
                            float whenToStopSpawning,
                            Vector3 offset,
                            bool waveSpawn)
	{
        EntityToSpawn newEnemy = new EntityToSpawn(enemyPrefab,
                             numberPerSpawn,
                             timeBetweenSpawns,
                             whenToStartSpawning,
                             whenToStopSpawning,
                             offset,
                             waveSpawn);

        EntityList.Add(newEnemy);
        
	}

    //Enemy variables in order to spawn them How I want them too
    [System.Serializable]
    public class EntityToSpawn
	{
        public GameObject enemyPrefab;
        public int numberPerSpawn;
        public float timeBetweenSpawns;
        public float whenToStartSpawning;
        public float whenToStopSpawning;
        public Vector3 offset;
        public bool canSpawn;
        public bool waveSpawn;

        public EntityToSpawn(GameObject enemyPrefab, 
                            int numberPerSpawn, 
                            float timeBetweenSpawns, 
                            float whenToStartSpawning, 
                            float whenToStopSpawning, 
                            Vector3 offset,
                            bool waveSpawn)
		{
            this.enemyPrefab = enemyPrefab;
            this.numberPerSpawn = numberPerSpawn;
            this.timeBetweenSpawns = timeBetweenSpawns;
            this.whenToStartSpawning = whenToStartSpawning;
            this.whenToStopSpawning = whenToStopSpawning;
            this.offset = offset;
            canSpawn = true;
            this.waveSpawn = waveSpawn;
		}
	}
}
