using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCS_Chunk
{
    #region Data

    private MCS_Block[,,] myBlocks;

    private List<int> myIndices = new List<int>();
    private List<Vector2> myUvs = new List<Vector2>();
    private List<Vector3> myVertices = new List<Vector3>();
    private List<Color> myColors = new List<Color>();

    private Vector3Int myChunkPos;
    private Vector3Int myWorldPos;

    private MCD_WorldData myWorldData;
    private Transform myTransform;

    public readonly List<Vector3Int> TopSoilBlocks = new List<Vector3Int>();

    #endregion

    #region Setters / Getters

    public Transform MyTransform
    {
        get { return myTransform; }
        set { myTransform = value; }
    }

    public MCS_Block[,,] Blocks
    {
        get { return myBlocks; }
        set { myBlocks = value; }
    }

    public List<int> Indices
    {
        get { return myIndices; }
        set { myIndices = value; }
    }

    public List<Vector2> Uvs
    {
        get { return myUvs; }
        set { myUvs = value; }
    }

    public List<Vector3> Vertices
    {
        get { return myVertices; }
        set { myVertices = value; }
    }

    public List<Color> Colors
    {
        get { return myColors; }
        set { myColors = value; }
    }

    public Vector3Int WorldPos
    {
        get { return myWorldPos; }
        set { myWorldPos = value; }
    }

    public MCS_Block GetBlock(int x, int y, int z)
    {
        bool isCoordInvalid = (x < 0 || x >= myWorldData.MyChunkSizeX)
            || (y < 0 || y >=  myWorldData.MyChunkSizeY)
            || (z < 0 || z >= myWorldData.MyChunkSizeZ);

        if (isCoordInvalid)
        {
            return null;
        }

        return myBlocks[x, y, z];
    }

    #endregion

    #region Methods

    public MCS_Chunk(MCD_WorldData aWorldData ,Vector3Int aChunkPos)
    {
        myChunkPos = aChunkPos;
        myWorldData = aWorldData;
        myWorldPos = new Vector3Int(myChunkPos.x * myWorldData.MyChunkSizeX, myChunkPos.y * myWorldData.MyChunkSizeY, myChunkPos.z * myWorldData.MyChunkSizeZ);
    }

    public void GenerateBlocks()
    {
        myBlocks = new MCS_Block[myWorldData.MyChunkSizeX, myWorldData.MyChunkSizeY, myWorldData.MyChunkSizeZ];

        for (int x = 0; x < myWorldData.MyChunkSizeX; x++)
        {
            for (int y = 0; y < myWorldData.MyChunkSizeY; y++)
            {
                for (int z = 0; z < myWorldData.MyChunkSizeZ; z++)
                {
                    Vector3Int pos = new Vector3Int(x, y, z);
                    myBlocks[x, y, z] = new MCS_Block(BlockType.Air, pos + WorldPos, this);
                    myBlocks[x, y, z].myWorldData = myWorldData;
                }
            }
        }
    }

    #endregion
}
