using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private GameObject warningEffectPrefab;
    [SerializeField] private float warningEffectDuration = 1f;
    [System.Serializable]
    public class SpawnInfo
    {
        public GameObject enemyPrefab;
        public Transform spawnLocation;
        public float spawnDelay = 0f;
    }

    public List<SpawnInfo> spawnSettings = new List<SpawnInfo>();

    void Start()
    {
        foreach (SpawnInfo spawnInfo in spawnSettings)
        {
            StartCoroutine(SpawnEnemyWithWarning(spawnInfo));
        }
    }

    private IEnumerator SpawnEnemyWithWarning(SpawnInfo spawnInfo)
    {
        if (warningEffectPrefab != null)
        {
            GameObject warningEffect = Instantiate(warningEffectPrefab, spawnInfo.spawnLocation.position, Quaternion.identity);

            Destroy(warningEffect, warningEffectDuration);

            yield return new WaitForSeconds(warningEffectDuration);
        }
        yield return new WaitForSeconds(spawnInfo.spawnDelay);

        Instantiate(spawnInfo.enemyPrefab, spawnInfo.spawnLocation.position, Quaternion.identity);
    }
}
