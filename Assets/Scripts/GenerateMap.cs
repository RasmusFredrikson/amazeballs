using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class GenerateMap : MonoBehaviour {

    Mesh mesh;

	// Use this for initialization
	void Start () {
        mesh = this.GetComponent<MeshFilter>().mesh;
        Destroy(this.gameObject.GetComponent<MeshCollider>());

        int circlePoints = 12; // 8, 12, 24, 36, 64
        float radius = 1f;
        float offsetX = 0;
        float offsetY = 0.5f;


        Vector3[] vertices = new Vector3[4 + circlePoints + 1];
        Vector3[] normals = new Vector3[4 + circlePoints + 1];
        int[] triangles = new int[3*(4 + circlePoints)];
        Vector2[] uv = new Vector2[4 + circlePoints + 1];

        mesh.Clear();

        // Corners
        vertices[0] = new Vector3(10, 0, -10);
        vertices[1] = new Vector3(10, 0, 10);
        vertices[2] = new Vector3(-10, 0, 10);
        vertices[3] = new Vector3(-10, 0, -10);

        int vert = 4;
        for (float angle = 0; angle < 2*Mathf.PI; angle+= Mathf.PI*2f/ circlePoints)
        {
            float x = Mathf.Cos(angle) * radius + offsetX;
            float y = Mathf.Sin(angle) * radius + offsetY; 

            vertices[vert] = new Vector3(x, 0, y);
            vert++;
        }

        int tri = 0;
        for (int i = 0; i < 4; i++)
        {
            triangles[tri * 3] = 4 + (i * circlePoints / 4);
            triangles[(tri * 3) + 1] = (i+1) % 4;
            triangles[(tri * 3) + 2] = i;
            tri++;

            for (int j = 0; j < ((circlePoints-4)/4) + 1; j ++)
            {
                triangles[tri * 3] = 4 + (i * circlePoints / 4) + j;
                triangles[(tri * 3) + 1] = 4 + (i * circlePoints / 4) + j+1;
                triangles[(tri * 3) + 2] = (i + 1) % 4;
                tri++;
            }
        }

        for (int i = 0; i < uv.Length; i++)
        {
            uv[i] = new Vector2(vertices[i].x, vertices[i].z);
            normals[i] = new Vector3(0, 1, 0);
        }

        Mesh newMesh = new Mesh();
        newMesh.vertices = vertices;
        newMesh.uv = uv;
        newMesh.triangles = triangles;
        //newMesh.normals = normals;
        mesh = newMesh;

        this.GetComponent<MeshFilter>().mesh = mesh;
        // Create new MeshCollider()
        this.gameObject.AddComponent<MeshCollider>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.G)) {
            Debug.Log("Generate Hole");

            Destroy(this.gameObject.GetComponent<MeshCollider>());

            Vector3[] vertices = mesh.vertices;
            Vector3[] normals = mesh.normals;
            int[] triangles = mesh.triangles;
            Vector2[] uv = mesh.uv;

            int triangleIndex = Random.Range(0, triangles.Length/3);
            
            

            mesh.triangles = triangles;

            // Create new MeshCollider()
            this.gameObject.AddComponent<MeshCollider>();




            Debug.Log(vertices.Length);
            Debug.Log(normals.Length);
            Debug.Log(triangles.Length);
            Debug.Log(uv.Length);

            LogDebugFile(vertices.Length.ToString());
            LogDebugFile(normals.Length.ToString());
            LogDebugFile(triangles.Length.ToString());
            LogDebugFile(uv.Length.ToString());

            foreach (Vector3 item in vertices) { LogDebugFile(item.ToString()); }
            foreach (Vector3 item in normals) { LogDebugFile(item.ToString()); }
            foreach (int item in triangles) { LogDebugFile(item.ToString()); }
            foreach (Vector2 item in uv) { LogDebugFile(item.ToString()); }

        }


    }


    private static void LogDebugFile(string text)
    {
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter("D:/tmp.txt", true);
        writer.WriteLine(text);
        writer.Close();
    }
}
