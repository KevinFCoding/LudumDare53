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
    [SerializeField] GameObject _playerGFX;
    [SerializeField] SoundManager _soundManager;

    private bool _isPlayerOnWayPoint = false;
    private bool _isTranslatingToWaypoint = false;

    private List<WayPoint> _nextWaypoint;

    private void Awake()
    {
        _nextWaypoint = new();
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            //Destroy(other.gameObject);
            enemy.HasTouchedPlayer();
            _isInfected = true;
            PlayerIsInfected();
            //PlayerIsInfected(other.gameObject.GetComponent<Enemy>().getVirusName());
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

    public void StopPlayer()
    {
        _speed = 0;
    }

    public bool isPlayerInfected()
    {
        return _isInfected;
    }

    public void GameStarted()
    {
        StartCoroutine(GFXGoBackAnimation());
    }

    private void PlayerIsInfected()
    {
        _virusAroundPlayer.SetActive(true);
    }

    IEnumerator PlayerSpinAnimation(float timeOfSpin)
    {
        float time = 0;
        Quaternion baseRotation = _playerGFX.transform.localRotation;
        while (time < timeOfSpin)
        {
            time += Time.deltaTime;
            _playerGFX.transform.Rotate(Vector3.forward * 300 * Time.deltaTime, Space.Self);
            yield return null;
        };
        if (time >= timeOfSpin) {
            float rotationTime = 0;
            while(rotationTime < 1)
            {
                rotationTime += Time.deltaTime;
                _playerGFX.transform.rotation = Quaternion.Lerp(_playerGFX.transform.localRotation, baseRotation, rotationTime / 1);
                yield return null;
            }
        }
    }

    IEnumerator GFXGoBackAnimation()
    {
        float time = 0;
        StartCoroutine(PlayerSpinAnimation(2));

        Vector3 startPosition = _playerGFX.transform.position;
        Vector3 endPosition = gameObject.transform.position;

        Vector3 startScale = _playerGFX.transform.localScale;
        Vector3 endScale = new Vector3(2, 4, 4);
        while (time < 2)
        {
            time += Time.deltaTime;
            _playerGFX.transform.localPosition = Vector3.Lerp(startPosition, endPosition, time / 2);
            _playerGFX.transform.localScale = Vector3.Lerp(startScale, endScale, time / 2);
            yield return null;
        };
        if(time > 2)
        {
            _speed = 5;
        }
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
        StartCoroutine(PlayerSpinAnimation(timeToReach));
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
