using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailBox : MonoBehaviour
{
    [SerializeField] Material _mailBoxMat;

    private bool _isSpam;
    private bool _isWin;

    private bool _isLose;
    public void isSpam()
    {
        _isSpam = true;
        _isWin = false;
        _isLose = false;

        _mailBoxMat.color = Color.yellow;
    }
    public void isWin()
    {
        _isSpam = false;
        _isWin = true;
        _isLose = false;

        _mailBoxMat.color = Color.green;
    }
    public void isLose()
    {
        _isSpam = false;
        _isWin = false;
        _isLose = true;

        _mailBoxMat.color = Color.red;
    }
}
