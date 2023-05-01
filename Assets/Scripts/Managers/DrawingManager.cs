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

    //[SerializeField] private List<WayPoints> wayPointsList = new List<WayPoints>();
    private List<GameObject> lines = new List<GameObject>();

    private Vector3 threadStartPointPosition;
    private Vector3 threadEndPointPosition;

    private bool hasStartedDrawing;
    private bool _isPaused;

    private void Start()
    {
        lineController.SetCamera(_mainCamera);
        hasStartedDrawing = false;
    }
    void Update()
    {
        if (!_isPaused)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                // SphereCast au lieu d'un Raycast pour permettre d'être moins précis sur le clic sur le thread
                //if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                foreach (RaycastHit hit in Physics.SphereCastAll(ray, 1f))
                {
                    if ((hit.transform.gameObject.GetComponent<DeliveryThread>() != null || (hit.transform.gameObject.GetComponent<WayPoint>() != null && hit.transform.name != "StartWayPoint" && hit.transform.name != "EndWayPoint")) && threads.Contains(hit.transform.gameObject))
                    {
                        StartDrawing(hit);
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                //if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                foreach (RaycastHit hit in Physics.SphereCastAll(ray, 1f))
                {
                    if ((hit.transform.gameObject.GetComponent<DeliveryThread>() != null || (hit.transform.gameObject.GetComponent<WayPoint>() != null && hit.transform.name != "StartWayPoint" && hit.transform.name != "EndWayPoint")) && hasStartedDrawing)
                    {
                        StopDrawing(hit);
                    }
                    else
                    {
                        lineController.DeleteLine();
                    }
                }
            }
        }

    }
    public void GamePaused(bool isPaused)
    {
        _isPaused = isPaused;
        foreach (var thread in threads)
        {
            thread.GetComponent<DeliveryThread>().GamePaused(isPaused);
        }

    }

    public void StartDrawing(RaycastHit hit)
    {
        if (_isPaused) return;
        hasStartedDrawing = true;
        GameObject threadSelected = hit.transform.gameObject;
        threadStartPointPosition = threadSelected.transform.position;

        Vector3 startPosition = new Vector3(threadSelected.transform.position.x, threadSelected.transform.position.y, hit.point.z);
        lineController.StartLine(startPosition);
    }

    public void StopDrawing(RaycastHit hit)
    {
        hasStartedDrawing = false;
        GameObject threadSelected = hit.transform.gameObject;
        threadEndPointPosition = threadSelected.transform.position;

        Vector3 endPosition = new Vector3(threadSelected.transform.position.x, threadSelected.transform.position.y, hit.point.z);
        Vector3[] positions = lineController.StopLineAndGetWaypointsPositions(endPosition);

        if (positions[0] != null && positions[1] != null)
        {
            if (CanDraw() && !HasIntersection(positions))
            {
                AddLine(positions);
            }
        }
    }

    public void SetThread(List<GameObject> DeliveryThreads)
    {
        threads = DeliveryThreads;
    }

    private void AddLine(Vector3[] positions)
    {
        GameObject start = Instantiate(wayPointGO, positions[0], Quaternion.identity);
        GameObject end = Instantiate(wayPointGO, positions[1], Quaternion.identity);

        WayPoints wp = new WayPoints(start.transform, end.transform);

        LineRenderer line = Instantiate(this.line);
        lines.Add(line.gameObject);

        //wayPointsList.Add(wp);

        line.positionCount = 2;
        line.SetPosition(0, wp.startPoint.position);
        line.SetPosition(1, wp.endPoint.position);
        //line.SetPosition(1, new Vector3(wp.endPoint.position.x, 0, wp.endPoint.position.z));

        start.GetComponent<WayPoint>().SetPoints(new WayPoints(start.transform, end.transform));
        end.GetComponent<WayPoint>().SetPoints(new WayPoints(end.transform, start.transform));

        //Debug.Log("DELETE FIRST LI?E CHECK START");

        //if (lines.Count > 4)
        //{
        //    Debug.Log("LINE DELETE");

        //    GameObject lineToRemove = lines.First();
        //    if (lineToRemove != null)
        //    {
        //        lines.Remove(lineToRemove);
        //        Destroy(lineToRemove);
        //    }

        //    Debug.Log("LINE DELETETED");

        //    Debug.Log("WAYPOI?T DELETE");
        //    WayPoints wayPointsToRemove = wayPointsList.First();
        //    if (wayPointsToRemove != null)
        //    {
        //        wayPointsList.Remove(wayPointsToRemove);

        //        wayPointsToRemove.startPoint.gameObject.SetActive(false);
        //        wayPointsToRemove.endPoint.gameObject.SetActive(false);
        //    }

        //    Debug.Log("ZQYPOI?T DELETETED");
        //}
    }

    private bool HasIntersection(Vector3[] positions)
    {
        foreach (var item in lines)
        {
            LineRenderer lr = item.GetComponent<LineRenderer>();
            Vector3 intersection;
            if (LineHelper.HasIntersectionPoint2D(lr.GetPosition(0), lr.GetPosition(1), positions[0], positions[1], out intersection))
            {
                if (LineHelper.IsBetweenPoints(positions[0], positions[1], intersection))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool CanDraw()
    {
        int threadOneIndex = threads.FindIndex(t => t.transform.position == threadStartPointPosition);
        int threadTwoIndex = threads.FindIndex(t => t.transform.position == threadEndPointPosition);
        if ((threadOneIndex + 1 == threadTwoIndex) || (threadOneIndex - 1 == threadTwoIndex))
        {
            return true;
        }
        return false;
    }
}