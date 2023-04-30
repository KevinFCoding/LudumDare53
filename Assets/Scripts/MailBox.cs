using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailBox : MonoBehaviour
{
    [SerializeField] Material _mailBoxMat;
    [SerializeField] GameManager _gameManager;

    private bool _isSpam;
    private bool _isWin;
    private bool _isLose;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }
    public void isSpam()
    {
        _isSpam = true;
        _isWin = false;
        _isLose = false;

        gameObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 255);
    }
    public void isWin()
    {
        _isSpam = false;
        _isWin = true;
        _isLose = false;

        gameObject.GetComponent<MeshRenderer>().material.color = new Color(0, 255, 0);
    }
    public void isLose()
    {
        _isSpam = false;
        _isWin = false;
        _isLose = true;

        gameObject.GetComponent<MeshRenderer>().material.color = new Color(255, 0, 0);
    }

    public void BoxTouched()
    {
        string boxTouched = "";
        if (_isSpam) boxTouched = "spam";
        if (_isWin) boxTouched = "win";
        if (_isLose) boxTouched = "lose";
        _gameManager.PlayerHasDeliveredTheMail(boxTouched);
    }
}
