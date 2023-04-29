using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LineController : MonoBehaviour
{
    [SerializeField] private GameObject wayPointGO;
    private Camera _mainCamera;

    private LineRenderer drawLineRenderer;

    private Transform startPoint;
    private Transform endPoint;


    private void Awake()
    {
        drawLineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {

        if (Input.GetMouseButton(0) && endPoint != null)
        {
            Debug.Log("qshdsqjdkqshdkjqsd");
            Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("Mouse Position " + mousePosition);
            endPoint.transform.position = mousePosition;

            drawLineRenderer.SetPosition(1, new Vector3(mousePosition.x, 0, mousePosition.z));
        }
    }
    
    public void StartLine(Vector3 position)
    {
        //DeleteLine();

        GameObject startWayPoint = Instantiate(wayPointGO, position, Quaternion.identity);
        startWayPoint.name = "Start WayPoint";

        GameObject endWayPoint = Instantiate(wayPointGO, position, Quaternion.identity);
        endWayPoint.name = "End WayPoint";

        SetUpLine(startWayPoint.transform, endWayPoint.transform);
    }

    public WayPoints StopLine(Vector3 barPosition)
    {
        endPoint.position = new Vector3(barPosition.x, endPoint.position.y, endPoint.position.z);
        drawLineRenderer.SetPosition(1, endPoint.position);

        WayPoints wp = new WayPoints(startPoint, endPoint);
        
        DeleteLine();

        return wp;
    }

    public void DeleteLine()
    {
        drawLineRenderer.positionCount = 0;
        if(startPoint)
        {
            Destroy(startPoint.gameObject);
            startPoint = null;
        }

        if (endPoint)
        {
            Destroy(endPoint.gameObject);
            endPoint = null;
        }
    }

    public void SetCamera(Camera camera)
    {
        _mainCamera = camera;
    }

    void SetUpLine(Transform start, Transform end)
    {
        drawLineRenderer.positionCount = 2;

        this.startPoint = start;
        this.endPoint = end;

        Vector3 initialPosition = new Vector3(start.position.x, 0, start.position.z);
        Debug.Log("initialPosition" + initialPosition);

        drawLineRenderer.SetPosition(0, initialPosition);
        drawLineRenderer.SetPosition(1, initialPosition);
    }
}