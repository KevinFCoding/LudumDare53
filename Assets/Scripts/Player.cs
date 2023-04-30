using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] float _speed = 10;
    [SerializeField] bool _isInfected = false;
    [SerializeField] Material _playerMat;
    [SerializeField] GameObject _virusAroundPlayer;

    private bool _isPlayerOnWayPoint = false;
    private bool _isTranslatingToWaypoint = false;

    private List<WayPoint> _nextWaypoint;

    private void Awake()
    {
        _nextWaypoint = new();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Enemy>())
        {
            Destroy(other.gameObject);
            _isInfected = true;
            PlayerIsInfected(other.gameObject.GetComponent<Enemy>().getVirusName());
            //gameObject.GetComponent<MeshRenderer>().material = _infectedPlayerMat;
        }

        if (other.gameObject.GetComponent<MailBox>())
        {
            other.gameObject.GetComponent<MailBox>().BoxTouched();
            gameObject.GetComponent<Player>().enabled = false;
        }
        if (other.gameObject.GetComponent<WayPoint>())
        {
            if (other.gameObject.GetComponent<WayPoint>().IsActive && !_nextWaypoint.Contains(other.gameObject.GetComponent<WayPoint>()))
            {
                _nextWaypoint.Add(other.gameObject.GetComponent<WayPoint>());
            }
        }
    }

    private void Start()
    {
        StartCoroutine(PlayerForwardMovement());
    }

    private void Update()
    {
        if (!_isTranslatingToWaypoint && _nextWaypoint.Count > 0)
        {
            WayPoint fwp = _nextWaypoint.First();

            if (fwp != null)
            {
                if (fwp.gameObject.transform.position.x == transform.position.x && fwp.gameObject.transform.position.z >= transform.position.z)
                {
                    WayPoints wps = fwp.GetPoints();
                    if (wps == null)
                    {
                        return;
                    }
                    _isPlayerOnWayPoint = true;
                    StartCoroutine(PlayerTranslateToWaypoint(wps));
                }
                else
                {
                    _nextWaypoint.Remove(fwp);
                }
            }

        }
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

    private void PlayerIsInfected(string virusName)
    {
        //if(virusName == "HVirus")
    }

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
        _isTranslatingToWaypoint = true;
        _nextWaypoint.Remove(_nextWaypoint.First());
        waypoints.startPoint.gameObject.GetComponent<WayPoint>().IsActive = false;
        waypoints.endPoint.gameObject.GetComponent<WayPoint>().IsActive = false;
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
            _isTranslatingToWaypoint = false;
            StartCoroutine(PlayerForwardMovement());
        }

    }
}
