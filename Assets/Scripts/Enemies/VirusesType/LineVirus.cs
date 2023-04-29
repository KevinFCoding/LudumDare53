using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineVirus : Spawnable
{
    [SerializeField] float _speed = 3;
    void Update()
    {
        transform.Translate(-transform.forward * _speed * Time.deltaTime);
    }
}
