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

    public void StopLine()
    {
        GenerateMeshCollider(drawLineRenderer);
        if (!drawLineRenderer.GetComponent<Line>().collide)
        {
            Debug.Log("pas collide");
            AddLine(new WayPoints(startPoint, endPoint));
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

        Destroy(drawLineRenderer.gameObject.GetComponent<Rigidbody>());
        Destroy(drawLineRenderer.gameObject.GetComponent<MeshCollider>());
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

        GenerateMeshCollider(line);

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

    void GenerateMeshCollider(LineRenderer line)
    {
        MeshCollider collider = line.gameObject.AddComponent<MeshCollider>();
        Rigidbody rigidbody = line.gameObject.AddComponent<Rigidbody>();
        rigidbody.isKinematic = true;

        line.gameObject.transform.position = new Vector3(0,0, 0);

        Mesh mesh = new Mesh();
        mesh.name = "line-mesh";

        line.BakeMesh(mesh);

        collider.sharedMesh = mesh;
    }
}



//public class GameLinesController : MonoBehaviour
//{
//    private LineRenderer fullLineRenderer;

//    private void Awake()
//    {
//        fullLineRenderer = GetComponent<LineRenderer>();
//    }

//    public void AddLine(WayPoints wayPoints)
//    {
//        int currentPosition = fullLineRenderer.positionCount;

//        fullLineRenderer.SetPosition(currentPosition + 1, wayPoints.startPoint.position);
//        fullLineRenderer.SetPosition(currentPosition + 1, wayPoints.endPoint.position);
//    }
//}