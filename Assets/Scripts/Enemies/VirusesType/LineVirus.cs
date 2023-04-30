using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineVirus : Spawnable
{
    [SerializeField] float _speed = 3;

    private void Start()
    {
        name = "LVirus";
    }
    void Update()
    {
        transform.Translate(-transform.forward * _speed * Time.deltaTime);
    }

    public override void HasTouchedPlayer()
    {
        base.HasTouchedPlayer();
        Destroy(gameObject);
    }
}
