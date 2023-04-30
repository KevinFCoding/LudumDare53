using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SinusoidVirus : Spawnable
{
    [SerializeField] float _speed = 3;
    [SerializeField] float _frequency = 2;
    [SerializeField] float magnitude = .5f;
    private void Start()
    {
        name = "SVirus";
    }

    void Update()
    {

        Vector3 forwardMovement = _speed * Time.deltaTime * -transform.forward;
        Vector3 upMovement = Mathf.Sin(Time.time * _frequency) * magnitude * transform.up;

        transform.Translate(forwardMovement + upMovement * _speed * Time.deltaTime);
    }

    public override void HasTouchedPlayer()
    {
        base.HasTouchedPlayer();
        Destroy(gameObject);
    }
}