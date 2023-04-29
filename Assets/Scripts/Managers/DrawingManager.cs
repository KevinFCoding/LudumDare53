using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrawingManager : MonoBehaviour
{
    [SerializeField] private LineController lineController;
    [SerializeField] private LineRenderer line;
    [SerializeField] private Camera _mainCamera;

    //private List<GameObject> threads = new List<GameObject>();
    [SerializeField] List<GameObject> threads = new List<GameObject>();
    private List<GameObject> lines = new List<GameObject>();

    private GameObject threadStartPoint;
    private GameObject threadEndPoint;

    private void Start()
    {
        lineController.SetCamera(_mainCamera);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse DOWN");

            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.transform.gameObject.GetComponent<DeliveryThread>() != null && threads.Contains(hit.transform.gameObject))
                {
                    StartDrawing(hit);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Mouse UP");

            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.transform.gameObject.GetComponent<DeliveryThread>() != null)
                {
                    Debug.Log("On Bar UP");
                    StopDrawing(hit);
                }
                else
                {
                    Debug.Log("Out Bar UP");
                    lineController.DeleteLine();
                }
            }
            else
            {
                Debug.Log("Out Bar UP");
                lineController.DeleteLine();
            }
        }
    }


    public void StartDrawing(RaycastHit hit)
    {
        GameObject threadSelected = hit.transform.gameObject;
        threadStartPoint = threadSelected;

        Vector3 startPosition = new Vector3(threadSelected.transform.position.x,hit.point.y, hit.point.z); ;
        Debug.Log("start POSITION" + startPosition);
        lineController.StartLine(startPosition);
    }

    public void StopDrawing(RaycastHit hit)
    {
        GameObject threadSelected = hit.transform.gameObject;
        threadEndPoint = threadSelected;

        Vector3 endPosition = new Vector3(threadSelected.transform.position.x, hit.point.x, hit.point.z);
        WayPoints wp = lineController.StopLine(endPosition);

        if (CanDraw() && !HasIntersection(wp))
        {
            AddLine(wp);
        }
    }

    public void SetThread(List<GameObject> DeliveryThreads)
    {
        threads = DeliveryThreads;
    }

    private void AddLine(WayPoints wayPoints)
    {
        LineRenderer line = Instantiate(this.line);
        lines.Add(line.gameObject);

        line.positionCount = 2;
        line.SetPosition(0, wayPoints.startPoint.position);
        line.SetPosition(1, new Vector3(wayPoints.endPoint.position.x, 0, wayPoints.endPoint.position.z));

        if (lines.Count >= 10)
        {
            GameObject lineToRemove = lines.First();
            if (lineToRemove != null)
            {
                lines.Remove(lineToRemove);
                Destroy(lineToRemove);
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
                Debug.Log("Intersection " + intersection);
                if (LineHelper.IsBetweenPoints(wp.startPoint.position, wp.endPoint.position, intersection))
                {
                    Debug.Log("Intersect");
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

        if((threadOneIndex + 1 == threadTwoIndex) || (threadOneIndex - 1 == threadTwoIndex))
        {
            return true;
        }
        return false;
    }


}
