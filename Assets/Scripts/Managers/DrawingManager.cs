using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrawingManager : MonoBehaviour
{
    [SerializeField] private GameObject wayPointGO;
    [SerializeField] private LineController lineController;

    [SerializeField] private LineRenderer line;

    [SerializeField] private Camera _mainCamera;


    //private List<GameObject> threads = new List<GameObject>();
    [SerializeField] List<GameObject> threads = new List<GameObject>();

    private List<WayPoints> wayPointsList = new List<WayPoints>();
    private List<GameObject> lines = new List<GameObject>();

    private GameObject threadStartPoint;
    private GameObject threadEndPoint;

    private bool hasStartedDrawing;

    private void Start()
    {
        lineController.SetCamera(_mainCamera);
        hasStartedDrawing = false;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            // SphereCast au lieu d'un Raycast pour permettre d'être moins précis sur le clic sur le thread
            //if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            if (Physics.SphereCast(ray, 1f, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.transform.gameObject.GetComponent<DeliveryThread>() != null && threads.Contains(hit.transform.gameObject))
                {
                    StartDrawing(hit);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            //if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            if (Physics.SphereCast(ray, 2f, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.transform.gameObject.GetComponent<DeliveryThread>() != null && hasStartedDrawing)
                {
                    StopDrawing(hit);
                }
                else
                {
                    lineController.DeleteLine();
                }
            }
            else
            {
                lineController.DeleteLine();
            }
        }
    }


    public void StartDrawing(RaycastHit hit)
    {
        hasStartedDrawing = true;
        GameObject threadSelected = hit.transform.gameObject;
        threadStartPoint = threadSelected;

        Vector3 startPosition = new Vector3(threadSelected.transform.position.x, 0, hit.point.z);
        lineController.StartLine(startPosition);
    }

    public void StopDrawing(RaycastHit hit)
    {
        hasStartedDrawing = false;
        GameObject threadSelected = hit.transform.gameObject;
        threadEndPoint = threadSelected;

        Vector3 endPosition = new Vector3(threadSelected.transform.position.x, threadSelected.transform.position.y, hit.point.z);
        WayPoints wp = lineController.StopLine(endPosition);

        if (CanDraw() && !HasIntersection(wp) && wp != null)
        {
            AddLine(wp);
        }
    }

    public void SetThread(List<GameObject> DeliveryThreads)
    {
        threads = DeliveryThreads;
    }

    private void AddLine(WayPoints wp)
    {
        LineRenderer line = Instantiate(this.line);
        lines.Add(line.gameObject);
        wayPointsList.Add(wp);

        line.positionCount = 2;
        line.SetPosition(0, wp.startPoint.position);
        line.SetPosition(1, wp.endPoint.position);
        //line.SetPosition(1, new Vector3(wp.endPoint.position.x, 0, wp.endPoint.position.z));

        GameObject start = Instantiate(wayPointGO, wp.startPoint.position, Quaternion.identity);
        GameObject end = Instantiate(wayPointGO, wp.endPoint.position, Quaternion.identity);

        start.GetComponent<WayPoint>().SetPoints(new WayPoints(start.transform, end.transform));
        end.GetComponent<WayPoint>().SetPoints(new WayPoints(end.transform, start.transform));

        if (lines.Count >= 10)
        {
            GameObject lineToRemove = lines.First();
            if (lineToRemove != null)
            {
                lines.Remove(lineToRemove);
                Destroy(lineToRemove);
            }

            WayPoints wayPointsToRemove = wayPointsList.First();
            if (wayPointsToRemove != null)
            {
                wayPointsList.Remove(wayPointsToRemove);

                Destroy(wayPointsToRemove.startPoint);
                Destroy(wayPointsToRemove.endPoint);
            }
        }
    }

    private bool HasIntersection(WayPoints wp)
    {
        foreach (var item in lines)
        {
            LineRenderer lr = item.GetComponent<LineRenderer>();
            Vector3 intersection;
            if (LineHelper.HasIntersectionPoint2D(lr.GetPosition(0), lr.GetPosition(1), wp.startPoint.position, wp.endPoint.position, out intersection))
            {
                if (LineHelper.IsBetweenPoints(wp.startPoint.position, wp.endPoint.position, intersection))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool CanDraw()
    {
        int threadOneIndex = threads.FindIndex(t => t.transform.position == threadStartPoint.transform.position);
        int threadTwoIndex = threads.FindIndex(t => t.transform.position == threadEndPoint.transform.position);

        Debug.Log(threadOneIndex + " " + threadTwoIndex);

        if ((threadOneIndex + 1 == threadTwoIndex) || (threadOneIndex - 1 == threadTwoIndex))
        {
            return true;
        }
        return false;
    }
}