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
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip damageSound;
    [SerializeField] ParticleSystem _particules;

    [Header("GFX STRAFE")]
    [SerializeField] float _speedStrafe = 10f;
    [SerializeField] float _strafeFrequency;
    [SerializeField] float _strafeMagnitude;

    [Header("GFX SCALE")]
    [SerializeField] float _frequency = 1;
    [SerializeField] float  magnitude = .2f;

    [Header("TARGET INDICATOR")]
    [SerializeField] GameObject currentWinThread;
    [SerializeField] GameObject currentSpamThread;

    [SerializeField] SpriteRenderer targetSprite;
    [SerializeField] Sprite[] targetSprites;

    [SerializeField] GameObject girlFriendCursor;

    private bool _isPlayerOnWayPoint = false;
    private bool _isTranslatingToWaypoint = false;


    private List<WayPoint> _nextWaypoint;

    private void Awake()
    {
        _audioSource = GameObject.FindAnyObjectByType<SoundManager>().GetComponentInChildren<AudioSource>();
        _nextWaypoint = new();
        _particules = gameObject.GetComponentInChildren<ParticleSystem>();
        _particules.gameObject.SetActive(false);

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
        girlFriendCursor.SetActive(false);
        targetSprite.sprite = targetSprites[0];
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

        if (girlFriendCursor.activeSelf)
        {
            if (currentWinThread != null && !_isInfected)
            {
                girlFriendCursor.transform.LookAt(currentWinThread.transform.position);
            }

            if (currentSpamThread != null && _isInfected)
            {
                girlFriendCursor.transform.LookAt(currentSpamThread.transform.position);
            }
        }
    }

    public void StopPlayer()
    {
        _speed = 0;
        girlFriendCursor.SetActive(false);
        _particules.gameObject.SetActive(false);
        _particules.Stop();

    }

    public bool isPlayerInfected()
    {

        return _isInfected;
    }

    public void GameStarted()
    {
        StartCoroutine(GFXGoBackAnimation());
    }

    public void SetCurrentWinThread(GameObject winThread)
    {
        currentWinThread = winThread;
    }

    public void SetCurrentSpamThread(GameObject spamThread)
    {
        currentSpamThread = spamThread;
    }

    private void PlayerIsInfected()
    {
        _audioSource.PlayOneShot(damageSound, 5f);

        _virusAroundPlayer.SetActive(true);
        targetSprite.sprite = targetSprites[1];

    }
    IEnumerator PlayerHoverAnimation()
    {

        Vector3 baseLocalScale = _playerGFX.transform.localScale; 
        while (true)
        {

            // Change Scale for Breathing Effect
            _playerGFX.transform.localScale = _playerGFX.transform.localScale * Mathf.Sin(Time.time * _frequency) * magnitude + baseLocalScale;
            
            // Strafe tot give Movement
            Vector3 upAndDownMovement = Mathf.Cos(Time.time * _strafeFrequency) * _strafeMagnitude * transform.right;
            _playerGFX.transform.Translate(upAndDownMovement * _speedStrafe * Time.deltaTime);

            yield return null;
        }
    }

    IEnumerator PlayerSpinAnimation(float timeOfSpin)
    {
        float time = 0;
        Quaternion baseRotation = _playerGFX.transform.localRotation;
        while (time < timeOfSpin)
        {
            time += Time.deltaTime;
            _playerGFX.transform.Rotate(Vector3.forward * 250 * Time.deltaTime, Space.Self);
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
        StartCoroutine(PlayerSpinAnimation(1.5f));

        Vector3 startPosition = _playerGFX.transform.position;
        Vector3 endPosition = gameObject.transform.position;

        Vector3 startScale = _playerGFX.transform.localScale;
        Vector3 endScale = new Vector3(4, 4, 4);
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
            girlFriendCursor.SetActive(true);
            StartCoroutine(PlayerHoverAnimation());
            _particules.gameObject.SetActive(true);
            _particules.Play();

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
