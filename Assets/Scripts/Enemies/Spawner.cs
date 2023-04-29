using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] _spawners;

    public void ActivateSpawners()
    {
        _spawners[Random.Range(0, _spawners.Length)].SetActive(true);
    }
}
