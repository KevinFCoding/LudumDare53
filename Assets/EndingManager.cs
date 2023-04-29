using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingManager : MonoBehaviour
{

    [SerializeField] GameObject _winParticules;
    [SerializeField] GameObject _victoryPanel;
    [SerializeField] GameObject _victoryImage;
    void Start()
    {
        Win();
    }

    void Update()
    {
        
    }

    public void Win()
    {
        _victoryPanel.SetActive(true);
        StartCoroutine(ActiveEndingPanel());
    }
   

    IEnumerator ActiveEndingPanel()
    {
        yield return new WaitForSeconds(1.6f);
        _victoryImage.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _winParticules.SetActive(true);

        yield break;
    }

}
