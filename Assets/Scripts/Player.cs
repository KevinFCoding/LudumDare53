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

    private bool _isPlayerOnWayPoint = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Enemy>())
        {
            Destroy(other.gameObject);
            _isInfected = true;
            gameObject.GetComponent<MeshRenderer>().material = _infectedPlayerMat;
        }

        if (other.gameObject.GetComponent<MailBox>())
        {
            other.gameObject.GetComponent<MailBox>().BoxTouched();
            gameObject.GetComponent<Player>().enabled = false;
        }
        if (other.gameObject.GetComponent<WayPoint>())
        {
            WayPoints wps = other.gameObject.GetComponent<WayPoint>().GetPoints();
            if (wps == null)
            {
                return;
            }
            _isPlayerOnWayPoint = true;
            StartCoroutine(PlayerTranslateToWaypoint(wps));
        }
    }

    private void Start()
    {
        StartCoroutine(PlayerForwardMovement());
    }

    private void Update()
    {
    }

    public bool isPlayerInfected()
    {
        return _isInfected;
    }

    //private void CheckForPlayerInput()
    //{
    //    if(Input.GetKeyDown(KeyCode.D))
    //    {
    //        transform.position = new Vector3(transform.position.x + 3, transform.position.y, transform.position.z);
    //    }
    //    if (Input.GetKeyDown(KeyCode.Q))
    //    {
    //        transform.position = new Vector3(transform.position.x - 3, transform.position.y, transform.position.z);
    //    }
    //}

    IEnumerator PlayerForwardMovement()
    {
        while (!_isPlayerOnWayPoint)
        {
            transform.Translate(transform.forward * _speed * Time.deltaTime);
            yield return null;
        };
    }

    IEnumerator PlayerTranslateToWaypoint(WayPoints waypoints)
    {
        float time = 0;
        Vector3 startPos = transform.position;
        Vector3 endPos = waypoints.startPoint.position;
        float distance = Vector3.Distance(startPos, endPos);
        float timeToReach = distance / _speed;
        while (time < timeToReach)
        {
            time += Time.deltaTime;
            //transform.Translate((waypoints.endPoint.position - waypoints.startPoint.position).normalized * _speed);
            transform.position = Vector3.Lerp(startPos, endPos, time / timeToReach);
            yield return null;
        };
        if (time >= timeToReach)
        {
            StartCoroutine(PlayerWaypointMovement(waypoints));
        }
    }

    IEnumerator PlayerWaypointMovement(WayPoints waypoints)
    {
        float time = 0;
        Debug.Log(waypoints.startPoint + " " + waypoints.endPoint);
        Vector3 startPos = waypoints.startPoint.position;
        Vector3 endPos = waypoints.endPoint.position;
        waypoints.startPoint.gameObject.SetActive(false);
        waypoints.endPoint.gameObject.SetActive(false);
        float distance = Vector3.Distance(startPos, endPos);
        float timeToReach = distance / _speed;
        while (time < timeToReach)
        {
            time += Time.deltaTime;
            //transform.Translate((waypoints.endPoint.position - waypoints.startPoint.position).normalized * _speed);
            transform.position = Vector3.Lerp(startPos, endPos, time / timeToReach);
            yield return null;
        };
        if (time >= timeToReach)
        {
            _isPlayerOnWayPoint = false;

            StartCoroutine(PlayerForwardMovement());
        }
    }
}
