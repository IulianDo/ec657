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
            Coroutine spawnEnemy = StartCoroutine(SpawnEnemy(currentEnemy));
            StartCoroutine(StopSpawning(currentEnemy, spawnEnemy));
        }
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
            for (int i = 0; i < currentEnemy.numberPerSpawn; i++)
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

                GameObject spawnedEnemy = Instantiate(currentEnemy.enemyPrefab, offset + currentEnemy.offset, rotation);
                Vector3 LookPosition = new Vector3(transform.position.x,
                                                    spawnedEnemy.transform.position.y,
                                                    transform.position.z);

                spawnedEnemy.transform.LookAt(LookPosition);
            }
            yield return new WaitForSeconds(currentEnemy.timeBetweenSpawns);
        }
    }
	#endregion

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

        public EntityToSpawn(GameObject enemyPrefab, int numberPerSpawn, float timeBetweenSpawns, float whenToStartSpawning, float whenToStopSpawning, Vector3 offset)
		{
            this.enemyPrefab = enemyPrefab;
            this.numberPerSpawn = numberPerSpawn;
            this.timeBetweenSpawns = timeBetweenSpawns;
            this.whenToStartSpawning = whenToStartSpawning;
            this.whenToStopSpawning = whenToStopSpawning;
            this.offset = offset;
            canSpawn = true;
		}
	}
}
