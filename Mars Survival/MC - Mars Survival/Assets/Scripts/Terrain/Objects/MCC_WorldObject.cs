using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCC_WorldObject : MonoBehaviour
{
    [SerializeField] private int myTerrainHeight = 32;
    [SerializeField] private int myTerrainDepth = 128;
    [SerializeField] private int myTerrainWidth = 64;
    [SerializeField] private int myTerrainLength = 64;

    [SerializeField] private Vector3Int myChunkSize = new Vector3Int(32, 32+128, 32);

    [SerializeField] private GameObject myChunkPrefab;

    [SerializeField] private Texture myWorldTextureAtlas;

    [SerializeField] private float myVoxelSize = 0.5f;

    public MCD_WorldData myWorldData;
    private MCS_World myWorld;
    private MCG_TerrainGeneration terrainGeneration;
    private MCG_DecorationGeneration decorationGeneration;

    void Start()
    {
        Vector3Int terrainSize = new Vector3Int(myTerrainWidth, myTerrainHeight + myTerrainDepth, myTerrainLength);
        myWorldData = new MCD_WorldData(terrainSize, myChunkSize, myVoxelSize);
        myWorld = new MCS_World(myWorldData);
        terrainGeneration = new MCG_TerrainGeneration();
        decorationGeneration = new MCG_DecorationGeneration(myWorldData);

        myWorld.InitChunkGrid();
        myWorldData.GenerateUVCoordinatesForAllBlocks();
        CreateChunksObject();
        GenerateChunkMesh();
    }

    private void CreateChunksObject()
    {
        for (int x = 0; x < myWorldData.MyTerrainSizeX / myWorldData.MyChunkSizeX; x++)
        {
            for (int y = 0; y < myWorldData.MyTerrainSizeY / myWorldData.MyChunkSizeY; y++)
            {
                for (int z = 0; z < myWorldData.MyTerrainSizeZ / myWorldData.MyChunkSizeZ; z++)
                {
                    Vector3Int chunkPos = new Vector3Int(x, y, z);
                    MCS_Chunk chunk = myWorldData.GetChunkByIndex(chunkPos);
                    CreatePrefabForChunk(chunk);
                }
            }
        }
    }

    private void CreatePrefabForChunk(MCS_Chunk chunk)
    {
        Vector3 chunkGameObjectPosition = new Vector3(chunk.WorldPos.x, chunk.WorldPos.y, chunk.WorldPos.z) * myVoxelSize;
        Transform chunkGameObject = Instantiate(myChunkPrefab, chunkGameObjectPosition, Quaternion.identity).transform;
        chunkGameObject.parent = transform;
        chunkGameObject.name = chunk.ToString();

        MCC_ChunkObject chunkGameObjectScript = chunkGameObject.GetComponent<MCC_ChunkObject>();
        chunkGameObjectScript.MyTexture = myWorldTextureAtlas;
        chunk.MyTransform = chunkGameObject;
        chunkGameObject.localScale = new Vector3(1, 1, 1) * myVoxelSize;

        terrainGeneration.GenerateChunk(myWorldData, chunk);
        decorationGeneration.GenerateDecoration(chunk);
    }

    private void GenerateChunkMesh()
    {
        for (int x = 0; x < myWorldData.MyTerrainSizeX / myWorldData.MyChunkSizeX; x++)
        {
            for (int y = 0; y < myWorldData.MyTerrainSizeY / myWorldData.MyChunkSizeY; y++)
            {
                for (int z = 0; z < myWorldData.MyTerrainSizeZ / myWorldData.MyChunkSizeZ; z++)
                {
                    Vector3Int chunPos = new Vector3Int(x, y, z);
                    MCS_Chunk chunk = myWorldData.GetChunkByIndex(chunPos);

                    MCC_ChunkObject chunkGameObjectScript = chunk.MyTransform.GetComponent<MCC_ChunkObject>();
                    
                    myWorldData.MyChunkMesh.GenerateChunkMeshData(chunk);
                    chunkGameObjectScript.CreateChunkGameObjectMesh(chunk);
                }
            }
        }
    }
}
