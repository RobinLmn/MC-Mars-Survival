using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCC_ChunkObject : MonoBehaviour
{
    private Texture myTexture;
    private Shader myShader;

    private MeshFilter myMeshFilter;
    private MeshCollider myMeshCollider;
    private MeshRenderer myMeshRenderer;


    public Texture MyTexture
    {
        get { return myTexture; }
        set { myTexture = value; }
    }

    public void CreateChunkGameObjectMesh(MCS_Chunk chunk)
    {
        myMeshFilter = GetOrCreateComponent<MeshFilter>();
        myMeshCollider = GetOrCreateComponent<MeshCollider>();
        myMeshRenderer = GetOrCreateComponent<MeshRenderer>();

        //myMeshRenderer.material.shader = myShader;
        myMeshRenderer.material.mainTexture = myTexture;
        //transform.position = new Vector3(chunk.WorldPos.x, chunk.WorldPos.y, chunk.WorldPos.z;

        myMeshFilter.mesh.Clear();
        myMeshFilter.mesh.vertices = chunk.Vertices.ToArray();
        myMeshFilter.mesh.uv = chunk.Uvs.ToArray();
        myMeshFilter.mesh.colors = chunk.Colors.ToArray();
        myMeshFilter.mesh.triangles = chunk.Indices.ToArray();
        myMeshCollider.sharedMesh = null;
        myMeshCollider.sharedMesh = myMeshFilter.mesh;

        myMeshFilter.mesh.RecalculateNormals();

        chunk.Vertices = new List<Vector3>();
        chunk.Uvs = new List<Vector2>();
        chunk.Colors = new List<Color>();
        chunk.Indices = new List<int>();
    }

    public T GetOrCreateComponent<T>() where T : Component
    {
        T component = gameObject.GetComponent<T>();
        if (component == null)
        {
            component = gameObject.AddComponent<T>();
        }
        return component;
    }
}
