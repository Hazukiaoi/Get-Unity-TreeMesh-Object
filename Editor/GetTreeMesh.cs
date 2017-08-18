using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GetTreeMesh : EditorWindow {

    MeshFilter sourceMesh;
    MeshRenderer sourceMeshRender;

    [MenuItem("BakeTools/GetTreeMesh")]
    static public void GetMesh()
    {
        GetTreeMesh window = EditorWindow.GetWindow<GetTreeMesh>();
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("SourceTreeMeshFilter:");
        sourceMesh =  (MeshFilter)EditorGUILayout.ObjectField(sourceMesh,typeof(MeshFilter));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("SourceTreeMeshRenderer:");
        sourceMeshRender = (MeshRenderer)EditorGUILayout.ObjectField(sourceMeshRender, typeof(MeshRenderer));
        GUILayout.EndHorizontal();

        if(GUILayout.Button("Get!"))
        {
            Build();
        }
    }

    //get tree Mesh
    private void Build()
    {
        Mesh newMesh = new Mesh();

        int[][] subMeshIndices = new int[sourceMesh.sharedMesh.subMeshCount][];
        int[][] subMeshTriangles = new int[sourceMesh.sharedMesh.subMeshCount][];
        MeshTopology[] subMeshTopology = new MeshTopology[sourceMesh.sharedMesh.subMeshCount];
        List<Vector3> normals = new List<Vector3>();
        List<Vector4> tangents = new List<Vector4>();
        List<Vector2> uv0s = new List<Vector2>();
        List<Vector2> uv1s = new List<Vector2>();
        List<Vector3> vertices = new List<Vector3>();

        //get
        for (int i = 0; i < subMeshIndices.Length; i++)
        {
            subMeshIndices[i] = sourceMesh.sharedMesh.GetIndices(i);
            subMeshTriangles[i] = sourceMesh.sharedMesh.GetTriangles(i);
            subMeshTopology[i] = sourceMesh.sharedMesh.GetTopology(i);

        }

        sourceMesh.sharedMesh.GetNormals(normals);

        sourceMesh.sharedMesh.GetTangents(tangents);

        sourceMesh.sharedMesh.GetUVs(0,uv0s);
        sourceMesh.sharedMesh.GetUVs(1,uv1s);

        sourceMesh.sharedMesh.GetVertices(vertices);

        //set

        newMesh.SetVertices(vertices);
        newMesh.subMeshCount = subMeshTriangles.Length;
        for (int i = 0;i< subMeshTriangles.Length; i++)
        {
            newMesh.SetIndices(subMeshIndices[i], subMeshTopology[i], i);
        }
        for (int i = 0; i < subMeshTriangles.Length; i++)
        {
            newMesh.SetTriangles(subMeshTriangles[i], i);
        }
        newMesh.SetNormals(normals);
        newMesh.SetTangents(tangents);
        newMesh.SetUVs(0, uv0s);
        newMesh.SetUVs(1, uv1s);

        GameObject g = new GameObject();
        var mf = g.AddComponent<MeshFilter>();
        var mr = g.AddComponent<MeshRenderer>();

        mf.mesh = newMesh;
        mr.materials = sourceMeshRender.sharedMaterials;

        g.name = "Bake Tree";

    }
}
