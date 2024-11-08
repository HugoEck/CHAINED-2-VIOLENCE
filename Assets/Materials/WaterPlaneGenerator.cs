using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPlaneGenerator : MonoBehaviour
{

    [SerializeField] float size = 1;
    [SerializeField] int gridSize = 16;

    private MeshFilter filter;

    // Start is called before the first frame update
    void Start()
    {
        filter = GetComponent<MeshFilter>();
        filter.mesh = GenerateMesh();
    }

    private Mesh GenerateMesh()
    {
        Mesh m = new Mesh();

        var vericies = new List<Vector3>();
        var normals = new List<Vector3>();
        var uvs = new List<Vector2>();

        for(int x = 0; x < gridSize + 1; x++ )
        {
            for(int y = 0; y < gridSize +1; y++ )
            {
                vericies.Add(new Vector3(-size * 0.5f + size * (x / ((float)gridSize)), 0, -size * 0.5f + size * (y / ((float)gridSize))));
                normals.Add(Vector3.up);
                uvs.Add(new Vector2(x / (float)gridSize, y / (float)gridSize));
            }
        }

        var triangles = new List<int>();
        var vertcount = gridSize + 1;
        for(int i = 0; i < vertcount * vertcount - vertcount; i++)
        {
            if((i + 1) % vertcount == 0)
            {
                continue;
            }
            triangles.AddRange(new List<int>()
            {
                i+1+vertcount, i+vertcount,i,i,i+1,i+vertcount+1
            });
        }

        m.SetVertices(vericies);
        m.SetNormals(normals);
        m.SetTriangles(triangles,0);

        return m;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}