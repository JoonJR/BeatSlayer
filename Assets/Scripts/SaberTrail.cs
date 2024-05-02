using UnityEngine;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SaberTrail : MonoBehaviour
{
    
    public Transform basePoint;
    public Transform tipPoint;
    public float trailTime = 0.5f; // Duration in seconds for the trail to fade
    public Material trailMaterial;

    public Mesh trailMesh;
    private Vector3[] vertices;
    private int[] triangles;
    private Vector3[] lastPositions;
    private float lastTime;

    void Start()
    {
        trailMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = trailMesh;
        GetComponent<MeshRenderer>().material = trailMaterial;

        vertices = new Vector3[4];
        triangles = new int[6] { 0, 1, 2, 2, 1, 3 };
    }

    void Update()
    {
        if (Time.time - lastTime > trailTime / 2)
        {
            if (lastPositions == null)
            {
                lastPositions = new Vector3[2] { basePoint.position, tipPoint.position };
                return;
            }

            UpdateTrail(lastPositions[0], lastPositions[1], basePoint.position, tipPoint.position);
            lastPositions[0] = basePoint.position;
            lastPositions[1] = tipPoint.position;
            lastTime = Time.time;
        }
    }

    void UpdateTrail(Vector3 lastBase, Vector3 lastTip, Vector3 currentBase, Vector3 currentTip)
    {
        vertices[0] = lastBase;
        vertices[1] = lastTip;
        vertices[2] = currentBase;
        vertices[3] = currentTip;

        trailMesh.Clear();
        trailMesh.vertices = vertices;
        trailMesh.triangles = triangles;
        trailMesh.RecalculateBounds();
    }
}