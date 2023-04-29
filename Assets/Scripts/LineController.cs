using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LineController : MonoBehaviour
{
    [SerializeField] private GameObject wayPointGO;
    [SerializeField]  private LineRenderer line;

    private List<GameObject> lines = new List<GameObject>();

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
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            endPoint.transform.position = mousePosition;

            drawLineRenderer.SetPosition(1, new Vector3(mousePosition.x, mousePosition.y, 0));
        }
    }
    
    public void StartLine(Vector2 position)
    {
        DeleteLine();

        GameObject startWayPoint = Instantiate(wayPointGO, position, Quaternion.identity);
        startWayPoint.name = "Start WayPoint";

        GameObject endWayPoint = Instantiate(wayPointGO, position, Quaternion.identity);
        endWayPoint.name = "End WayPoint";

        SetUpLine(startWayPoint.transform, endWayPoint.transform);
    }

    public void StopLine(Vector3 barPosition)
    {
        endPoint.position = new Vector3(barPosition.x, endPoint.position.y, endPoint.position.z);
        drawLineRenderer.SetPosition(1, endPoint.position);


        Debug.Log("STOP " + startPoint.position+" "+endPoint.position+" "+drawLineRenderer.GetPosition(0)+" "+drawLineRenderer.GetPosition(1));

        WayPoints wp = new WayPoints(startPoint, endPoint);
        if (!HasIntersection(wp))
        {
            AddLine(wp);
        }
        
        DeleteLine();
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

    void SetUpLine(Transform start, Transform end)
    {
        drawLineRenderer.positionCount = 2;

        this.startPoint = start;
        this.endPoint = end;

        drawLineRenderer.SetPosition(0, new Vector3(start.position.x, start.position.y,0));
    }

    void AddLine(WayPoints wayPoints)
    {
        LineRenderer line = Instantiate(this.line);
        lines.Add(line.gameObject);

        line.positionCount = 2;
        line.SetPosition(0, wayPoints.startPoint.position);
        line.SetPosition(1, new Vector3(wayPoints.endPoint.position.x,wayPoints.endPoint.position.y,0));

        //GenerateMeshCollider(line);

        if(lines.Count >= 10)
        {
            GameObject lineToRemove = lines.First();
            if(lineToRemove != null)
            {
                lines.Remove(lineToRemove);
                Destroy(lineToRemove);
            }
        }
    }

    bool HasIntersection(WayPoints wp)
    {
        foreach (var item in lines)
        {
            LineRenderer lr = item.GetComponent<LineRenderer>();
            Vector3 intersection;
            if(LineHelper.HasIntersectionPoint2D(lr.GetPosition(0), lr.GetPosition(1), wp.startPoint.position, wp.endPoint.position, out intersection))
            {
                Debug.Log("Intersection " + intersection); 
                if(LineHelper.IsBetweenPoints(wp.startPoint.position, wp.endPoint.position, intersection))
                {
                    Debug.Log("Intersect");
                    return true;
                }      
            }
        }
        return false;
    }
    void GenerateMeshCollider(LineRenderer lr)
    {
        MeshCollider collider = lr.gameObject.GetComponent<MeshCollider>();
        if (collider == null) {

            collider = lr.gameObject.AddComponent<MeshCollider>();
            lr.gameObject.transform.position = new Vector3(0, 0, 0);

            collider.convex = true;
            collider.isTrigger = true;
        };

        Rigidbody rigidbody = lr.gameObject.GetComponent<Rigidbody>();
        if (rigidbody == null) {

            rigidbody = lr.gameObject.AddComponent<Rigidbody>();
            rigidbody.isKinematic = true;   

       };

        Mesh mesh = new Mesh();
        mesh.name = "line-mesh";

        lr.BakeMesh(mesh);
        collider.sharedMesh = mesh;
    }
}