using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LineController : MonoBehaviour
{
    [SerializeField] private GameObject drawingWayPointGO;
    private Camera _mainCamera;

    private LineRenderer drawLineRenderer;

    private GameObject startPoint;
    private GameObject endPoint;


    private void Awake()
    {
        drawLineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {

        if (Input.GetMouseButton(0) && endPoint != null)
        {
            Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            endPoint.transform.position = new Vector3(mousePosition.x, 0 , mousePosition.z);
            drawLineRenderer.SetPosition(1, new Vector3(mousePosition.x, 0, mousePosition.z));
        }
    }

    public void StartLine(Vector3 position)
    {
        DeleteLine();
        if (!HasWayPointNear(position))
        {
            GameObject startWayPoint = Instantiate(drawingWayPointGO, position, Quaternion.identity);
            startWayPoint.name = "StartWayPoint";
            GameObject endWayPoint = Instantiate(drawingWayPointGO, position, Quaternion.identity);
            endWayPoint.name = "EndWayPoint";
            SetUpLine(startWayPoint, endWayPoint);
        }
    }

    public Vector3[] StopLineAndGetWaypointsPositions(Vector3 endPosition)
    {
        Vector3[] positions = new Vector3[2];
        if (endPoint != null)
        {
            endPoint.transform.position = endPosition;
            drawLineRenderer.SetPosition(1, endPoint.transform.position);

            if (!HasWayPointNear(endPoint.transform.position))
            {
                positions[0] = startPoint.transform.position;
                positions[1] =  endPoint.transform.position;
            }
        }

        DeleteLine();

        return positions;
    }

    public void DeleteLine()
    {
        drawLineRenderer.positionCount = 0;
        if (startPoint)
        {
            Destroy(startPoint);
            startPoint = null;
        }

        if (endPoint)
        {
            Destroy(endPoint);
            endPoint = null;
        }
    }

    public void SetCamera(Camera camera)
    {
        _mainCamera = camera;
    }

    void SetUpLine(GameObject start, GameObject end)
    {
        drawLineRenderer.positionCount = 2;

        this.startPoint = start;
        this.endPoint = end;

        Vector3 initialPosition = new Vector3(start.transform.position.x, 0, start.transform.position.z);
        drawLineRenderer.SetPosition(0, initialPosition);
        drawLineRenderer.SetPosition(1, initialPosition);
    }

    bool HasWayPointNear(Vector3 center)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, 0f);
        List<Collider> list = hitColliders.Where(c => c.GetComponent<WayPoint>() != null && c.name != "StartWayPoint" && c.name != "EndWayPoint").ToList();

        return list.Count > 0 ? true : false;

    }
}