using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailBox : MonoBehaviour
{
    [SerializeField] string _nameBox = null;
    [SerializeField] GameObject _mailBoxGFX;
    [SerializeField] GameManager _gameManager;

    public string nameBox
    {
        get { return _nameBox; }
    }

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _nameBox = null;
    }
    public void MailBoxName(string nameBox)
    {
        _nameBox = nameBox;
        for (int i = 0; i < _mailBoxGFX.transform.childCount; i++)
        {
            GameObject icon = _mailBoxGFX.transform.GetChild(i).gameObject;
            if (_nameBox == icon.name) icon.SetActive(true);
        }
    }

    public void BoxTouched()
    {
        _gameManager.PlayerHasDeliveredTheMail(_nameBox);
    }
}
