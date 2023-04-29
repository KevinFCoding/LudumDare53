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
    // Start is called before the first frame update
    // Update is called once per frame

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Enemy>())
        {
            Destroy(other.gameObject);
            _isInfected = true;
            gameObject.GetComponent<MeshRenderer>().material = _infectedPlayerMat;
        }
    }

    void Update()
    {
        transform.Translate(transform.forward * _speed * Time.deltaTime);
        CheckForPlayerInput();
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
}
