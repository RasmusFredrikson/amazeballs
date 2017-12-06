using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSG : MonoBehaviour{

    Mesh m;

    Mesh mesh1;
    Mesh mesh2;

    private void Start()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        mesh2 = cube.GetComponent<MeshFilter>().mesh;

        GameObject cyl = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        mesh1 = cyl.GetComponent<MeshFilter>().mesh;

        CSGstart();
    }

    private class Node
    {
        public static Mesh m;
        public static List<Vector3> newVertices = new List<Vector3>();
        public Node front;
        public Node back;
        public TreeTriangle t;
        public Plane p;

        public Node(TreeTriangle tr)
        {
            p = new Plane(t.vert1, t.vert2, t.vert3);
            t = tr;
        }

        public void Add(TreeTriangle triangle)
        {
            
            float fa = p.GetDistanceToPoint(triangle.vert1);
            float fb = p.GetDistanceToPoint(triangle.vert1);
            float fc = p.GetDistanceToPoint(triangle.vert1);

            if (Mathf.Abs(fa) < 1E-2)
            {
                fa = 0;
            }
            if (Mathf.Abs(fb) < 1E-2)
            {
                fb = 0;
            }
            if (Mathf.Abs(fc) < 1E-2)
            {
                fc = 0;
            }

            if (fa <= 0 && fb <= 0 && fc <= 0)
            {
                if (back == null)
                {
                    back = new Node(triangle);
                    return;
                } else
                {
                    back.Add(triangle);
                    return;
                }
            }else if (fa >= 0 && fb >= 0 && fc >= 0)
            {
                if (front == null)
                {
                    front = new Node(triangle);
                    return;
                } else
                {
                    front.Add(triangle);
                    return;
                }
            }
            else
            {
                // Cut the triangle
                float a, b;
                p.Raycast(new Ray(triangle.vert1, (triangle.vert1 - triangle.vert3)), out a);
                p.Raycast(new Ray(triangle.vert2, (triangle.vert2 - triangle.vert3)), out b);

                Vector3 A = triangle.vert1 + (triangle.vert1 - triangle.vert3) * a;
                Vector3 B = triangle.vert2 + (triangle.vert2 - triangle.vert3) * b;

                int size = m.triangles.Length + newVertices.Count;

                newVertices.Add(A);
                newVertices.Add(B);
                
                TreeTriangle T1 = new TreeTriangle(triangle.vert1, triangle.vert2, A);
                TreeTriangle T2 = new TreeTriangle(triangle.vert2, B, A);
                TreeTriangle T3 = new TreeTriangle(A, B, triangle.vert3);

                if (fc >= 0)
                {
                    back.Add(T1);
                    back.Add(T2);
                    front.Add(T3);
                }
                else
                {
                    front.Add(T1);
                    front.Add(T2);
                    back.Add(T3);
                }
            }
            
        }
    }

    private struct TreeTriangle
    {
        public int tri1, tri2, tri3;
        public Vector3 vert1, vert2, vert3;

        public TreeTriangle(int t1, int t2, int t3, Vector3 v1, Vector3 v2, Vector3 v3)
        {
            tri1 = t1; tri2 = t2; tri3 = t3;
            vert1 = v1; vert2 = v2; vert3 = v3;
        }

        public TreeTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            tri1 = 0; tri2 = 0; tri3 = 0;
            vert1 = v1; vert2 = v2; vert3 = v3;
        }
    }

    public void CSGstart()
    {

        Node root = Build(mesh1);
        Node rootCyl = Build(mesh2);

        Traverse(root, null, null);
        Traverse(rootCyl, null, null);


    }

    private Node Build(Mesh mesh)
    {

        m = mesh;
        Node.m = m;

        int t1 = m.triangles[0], t2 = m.triangles[1], t3 = m.triangles[2];
        TreeTriangle t = new TreeTriangle(t1, t2, t3, m.vertices[t1], m.vertices[t2], m.vertices[t3]);
        Node root = new Node(t);

        for (int i = 3; i < m.triangles.Length; i = i + 3) {
            t1 = m.triangles[i]; t2 = m.triangles[i + 1]; t3 = m.triangles[i + 2];
            TreeTriangle triangle = new TreeTriangle(t1, t2, t3,m.vertices[t1], m.vertices[t2], m.vertices[t3]);
            root.Add(triangle);       
        }



        return root;
    }

    private void Traverse(Node root, List<int> triangles, List<Vector3> vertices)
    {
        if (root == null || (root.back == null && root.front == null))
        {
            return;
        }
        if (root == null)
        {

        } else
        {
            Debug.Log(root.t);
            Debug.Log("Triangle" + root.t.vert1 + " " + root.t.vert2 + " " + root.t.vert3);
            Traverse(root.front, triangles, vertices);
            Traverse(root.back, triangles, vertices);
        }
    }

}
