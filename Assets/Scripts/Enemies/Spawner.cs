using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] _spawners;
    // Start is called before the first frame update
    void Start()
    {
        //int bonusSpawn = undefined;
        //int rollDice = Random.Range(0, 100);
        //if (rollDice < 20)
        //{
        //    bonusSpawn = Random.Range(0, _spawners.Length);
        //    _spawners[bonusSpawn].SetActive(true);
        //}
        _spawners[Random.Range(0, _spawners.Length)].SetActive(true);
    }
}
