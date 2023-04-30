using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] GameObject[] _spawnablesVirus;

    private void Start()
    {
        SpawnVirus(_spawnablesVirus[Random.Range(0, _spawnablesVirus.Length)]);
    }

    private void SpawnVirus(GameObject virus)
    {
        Instantiate(virus, gameObject.transform.position, Quaternion.identity);
    }
}