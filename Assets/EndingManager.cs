using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingManager : MonoBehaviour
{
    [Header("Victory")]

    [SerializeField] GameObject _winParticules;
    [SerializeField] GameObject _victoryPanel;
    [SerializeField] GameObject _victoryImage;


    [Header("Defeate")]

    [SerializeField] GameObject _looseParticules;
    [SerializeField] GameObject _loosePanel;
    [SerializeField] GameObject _looseImage;
    void Start()
    {
        print("Start");
        //  Win();
        Loose();
    }

    void Update()
    {
        
    }

    public void Loose()
    {

        print("looseFunction");

        _loosePanel.SetActive(true);
        StartCoroutine(ActiveEndingLoosePanel());
    }

    public void Win()
    {
        _victoryPanel.SetActive(true);
        StartCoroutine(ActiveEndingWinPanel());
    }
   

    IEnumerator ActiveEndingLoosePanel()
    {
        print("looseCorouutine");

        yield return new WaitForSeconds(1.6f);
        _looseImage.SetActive(true);
        print("looseImage");
        yield return new WaitForSeconds(0.5f);
        _looseParticules.SetActive(true);

        yield break;
    }

    IEnumerator ActiveEndingWinPanel()
    {
        yield return new WaitForSeconds(1.6f);
        _victoryImage.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _winParticules.SetActive(true);

        yield break;
    }

}
