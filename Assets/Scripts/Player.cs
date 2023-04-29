using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] float _speed = 10;
    [SerializeField] bool _isInfected = false;
    [SerializeField] Material _playerMat;
    [SerializeField] Material _infectedPlayerMat;

    private bool _playerOnWayPoint = false;


    float qsd = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Enemy>())
        {
            Destroy(other.gameObject);
            _isInfected = true;
            gameObject.GetComponent<MeshRenderer>().material = _infectedPlayerMat;
        }

        //if (other.gameObject.GetComponent<>())
        //{

        //}
    }

    private void Start()
    {
        StartCoroutine(PlayerForwardMovement());
    }

    private void Update()
    {
    }

    private void CheckForPlayerInput()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            transform.position = new Vector3(transform.position.x + 3, transform.position.y, transform.position.z);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.position = new Vector3(transform.position.x - 3, transform.position.y, transform.position.z);
        }
    }

    IEnumerator PlayerForwardMovement()
    {
        while(!_playerOnWayPoint)
        {
            transform.Translate(transform.forward * _speed * Time.deltaTime);
            CheckForPlayerInput();
            yield return null;
        };
    }
    IEnumerator PlayerWaypointMovement()
    {
        while (_playerOnWayPoint)
        {
            //Vector3.Lerp();
            CheckForPlayerInput();
            yield return null;
        };
    }
}
