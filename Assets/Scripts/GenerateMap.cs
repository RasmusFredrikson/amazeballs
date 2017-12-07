using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class GenerateMap : MonoBehaviour {

    Mesh mesh;

    public List<Vector2> holes;

    int circlePoints = 24; // 8, 12, 24, 36, 64
    float radius = 0.5f;

    int boardX = 11;
    int boardY = 11;

    // Use this for initialization
    void Start () {
        mesh = this.GetComponent<MeshFilter>().mesh;
        Destroy(this.gameObject.GetComponent<MeshCollider>());

        mesh.Clear();


        Vector3[] planeVertices = new Vector3[(boardX) * (boardY)];
        List<int> planeTriangles = new List<int>();

        Vector3[] verticesAll = new Vector3[0];
        int[] trianglesAll = new int[0];
        for (int j = 0; j < boardY; j++) { 
            for (int i = 0; i < boardX; i++) {
                planeVertices[i + j * boardX] = new Vector3(i - boardX/2, 0, j-boardY/2);

                if (i > 0 && j > 0 && !holes.Contains(new Vector2(i, j)))
                {
                    planeTriangles.Add(i-1 + (j-1)* boardX);
                    planeTriangles.Add(i + j * boardX);
                    planeTriangles.Add(i + (j-1)* boardX);


                    planeTriangles.Add((i - 1) + (j - 1) * boardX);
                    planeTriangles.Add((i - 1) + j * boardX);
                    planeTriangles.Add(i + j * boardX);

                }


                if (holes.Contains(new Vector2(i, j)))
                {
                    Vector3[] verticesHole;
                    int[] trianglesHole;
                    Hole(i, j, planeVertices.Length + verticesAll.Length, out verticesHole, out trianglesHole);

                    Vector3[] concat = new Vector3[verticesAll.Length + verticesHole.Length];
                    for (int k = 0; k < verticesAll.Length; k++) {
                        concat[k] = verticesAll[k];
                    }
                    for (int k = verticesAll.Length; k < verticesAll.Length + verticesHole.Length; k++)
                    {
                        concat[k] = verticesHole[k - verticesAll.Length];
                    }
                    verticesAll = concat;

                    int[] concat2 = new int[trianglesAll.Length + trianglesHole.Length];
                    for (int k = 0; k < trianglesAll.Length; k++)
                    {
                        concat2[k] = trianglesAll[k];
                    }
                    for (int k = trianglesAll.Length; k < trianglesAll.Length + trianglesHole.Length; k++)
                    {
                        concat2[k] = trianglesHole[k - trianglesAll.Length];
                    }
                    trianglesAll = concat2;
                }


            }
        }

        Vector3[] vertices = new Vector3[planeVertices.Length + verticesAll.Length];
        Vector2[] uv = new Vector2[vertices.Length];
        Vector3[] normals = new Vector3[vertices.Length];
        int[] triangles = new int[planeTriangles.Count + trianglesAll.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            if (i < planeVertices.Length)
            {
                vertices[i] = planeVertices[i];
            } else
            {
                vertices[i] = verticesAll[i - planeVertices.Length];
            }
            
        }

        for (int i = 0; i < triangles.Length; i++)
        {
            if (i < planeTriangles.Count)
            {
                triangles[i] = planeTriangles[i];
            }
            else
            {
                triangles[i] = trianglesAll[i - planeTriangles.Count];
            }

        }

        for (int i = 0; i < vertices.Length; i++)
        {
            uv[i] = new Vector2(vertices[i].x, vertices[i].z);
            normals[i] = new Vector3(0, 1, 0);
        }

        Mesh newMesh = new Mesh();
        newMesh.vertices = vertices;
        newMesh.uv = uv;
        newMesh.triangles = triangles;
        newMesh.normals = normals;
        mesh = newMesh;

        this.GetComponent<MeshFilter>().mesh = mesh;
        // Create new MeshCollider()
        this.gameObject.AddComponent<MeshCollider>();
    }

    private void Hole(int ii, int jj, int triStart, out Vector3[] vertices, out int[] triangles)
    {
        vertices = new Vector3[circlePoints + 1];
        triangles = new int[3 * (4 + circlePoints)];

        float offsetX = (ii) - boardX/2 - radius;
        float offsetY = (jj) - boardY/2 - radius;

        int vert = 0; // 4;
        for (float angle = 0; angle < 2 * Mathf.PI; angle += Mathf.PI * 2f / circlePoints)
        {
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            vertices[vert] = new Vector3(x + offsetX, 0, y + offsetY);
            vert++;
        }

        int[] index = { ii + (jj - 1) * boardX, ii + jj * boardX, (ii - 1) + jj * boardX, (ii - 1) + (jj - 1) * boardX };

        for (int kk  = 0; kk < 4; kk++)
        {
            Debug.Log(index[kk]);
        }

        int tri = 0;
        for (int i = 0; i < 4; i++)
        {
            triangles[tri * 3] = triStart + (i * circlePoints / 4);
            triangles[(tri * 3) + 1] = index[(i + 1) % 4];
            triangles[(tri * 3) + 2] = index[i];
            tri++;

            for (int j = 0; j < ((circlePoints - 4) / 4) + 1; j++)
            {
                triangles[tri * 3] = triStart + (i * circlePoints / 4) + j;
                triangles[(tri * 3) + 1] = triStart + (i * circlePoints / 4) + j + 1;
                triangles[(tri * 3) + 2] = index[(i + 1) % 4];
                tri++;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        


    }


    private static void LogDebugFile(string text)
    {
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter("D:/tmp.txt", true);
        writer.WriteLine(text);
        writer.Close();
    }
}
