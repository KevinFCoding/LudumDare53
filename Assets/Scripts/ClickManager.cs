using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    [SerializeField] private LineController line;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse DOWN");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.transform.gameObject.GetComponent<Bar>() != null)
                {
                    Debug.Log("On Bar DOWN");

                    line.StartLine(hit.point);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Mouse UP");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.transform.gameObject.GetComponent<Bar>() != null)
                {
                    Debug.Log("On Bar UP");
                    line.StopLine();
                }
            }
            else
            {
                Debug.Log("Out Bar UP");
                line.DeleteLine();
            }
        }
    }
}
