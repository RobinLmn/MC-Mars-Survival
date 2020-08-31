using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCD_WorldData 
{
    private int myChunkSizeX = 32;
    private int myChunkSizeY = 160;
    private int myChunkSizeZ = 32;

    private int myTerrainSizeX = 64;
    private int myTerrainSizeY = 160;
    private int myTerrainSizeZ = 64;

    private int myTerrainHeight = 32;
    private int myTerrainDepth = 128;

    private MCS_ChunkMesh myChunkMesh;

    private float myVoxelSize = 0.5f;

    private readonly Dictionary<int, BlockUVCoordinates> m_BlockUVCoordinates;

    private MCS_Chunk[,,] myChunks;

    #region Getters/Setters

    public int MyTerrainHeight
    {
        get { return myTerrainHeight; }
        set { myTerrainHeight = value; }
    }

    public int MyTerrainDepth
    {
        get { return myTerrainDepth; }
        set { myTerrainDepth = value; }
    }

    public int MyChunkSizeX
    {
        get { return myChunkSizeX; }
        set { myChunkSizeX = value; }
    }

    public int MyChunkSizeY
    {
        get { return myChunkSizeY; }
        set { myChunkSizeY = value; }
    }

    public int MyChunkSizeZ
    {
        get { return myChunkSizeZ; }
        set { myChunkSizeZ = value; }
    }

    public int MyTerrainSizeX
    {
        get { return myTerrainSizeX; }
        set { myTerrainSizeX = value; }
    }

    public int MyTerrainSizeY
    {
        get { return myTerrainSizeY; }
        set { myTerrainSizeY = value; }
    }

    public int MyTerrainSizeZ
    {
        get { return myTerrainSizeZ; }
        set { myTerrainSizeZ = value; }
    }

    public MCS_Chunk[,,] MyChunks
    {
        get { return myChunks; }
    }

    public MCS_ChunkMesh MyChunkMesh
    {
        get { return myChunkMesh; }
    }

    public float MyVoxelSize
    {
        get { return myVoxelSize; }
    }

    #endregion

    #region Methods

    public MCD_WorldData(Vector3Int aTerrainSize, Vector3Int aChunkSize, float aVoxelSize)
    {
        MyTerrainSizeX = aTerrainSize.x;
        MyTerrainSizeY = aTerrainSize.y;
        MyTerrainSizeZ = aTerrainSize.z;

        MyChunkSizeX = aChunkSize.x;
        MyChunkSizeY = aChunkSize.y;
        myChunkSizeZ = aChunkSize.z;

        myVoxelSize = aVoxelSize;

        myChunkMesh = new MCS_ChunkMesh(this);

        myChunks = new MCS_Chunk[myTerrainSizeX / myChunkSizeX, myTerrainSizeY / myChunkSizeY, myTerrainSizeZ / myChunkSizeZ];

        m_BlockUVCoordinates = new Dictionary<int, BlockUVCoordinates>(5);
    }

    public MCS_Chunk GetChunkByIndex(Vector3Int aChunkPos)
    {
        return myChunks[aChunkPos.x, aChunkPos.y, aChunkPos.z];
    }

    public MCS_Block GetBlock(Vector3Int aWorldPos)
    {

        bool isCoordInvalid = (aWorldPos.x < 0 || aWorldPos.x >= MyTerrainSizeX)
            || (aWorldPos.y < 0 || aWorldPos.y >= MyTerrainSizeY)
            || (aWorldPos.z < 0 || aWorldPos.z >= MyTerrainSizeZ);

        if (isCoordInvalid)
        {
            return null;
        }

        int chunkX = aWorldPos.x / MyChunkSizeX;
        int chunkY = aWorldPos.y / MyChunkSizeY;
        int chunkZ = aWorldPos.z / MyChunkSizeZ;
        int blockX = aWorldPos.x % MyChunkSizeX;
        int blockY = aWorldPos.y % MyChunkSizeY;
        int blockZ = aWorldPos.z % MyChunkSizeZ;

        return myChunks[chunkX, chunkY, chunkZ].Blocks[blockX, blockY, blockZ];
       
    }

    public MCS_Block GetBlock(int x, int y, int z)
    {

        Vector3Int pos = new Vector3Int(x, y, z);
        return GetBlock(pos);

    }

    public Dictionary<int, BlockUVCoordinates> MyBlockUvCoordinates
    {
        get { return m_BlockUVCoordinates; }
    }

    public void GenerateUVCoordinatesForAllBlocks()
    {
        Vector2 dirt = new Vector2(0, 1);
        Vector2 grass = new Vector2(1, 0);
        Vector2 stone = new Vector2(0, 0);

        SetBlockUVCoordinates(BlockType.TopSoil, grass, grass, grass);
        SetBlockUVCoordinates(BlockType.Dirt, dirt, dirt, dirt);
        SetBlockUVCoordinates(BlockType.Stone, stone, stone, stone);
    }

    private void SetBlockUVCoordinates(BlockType blockType, Vector2 topIndex, Vector2 sideIndex, Vector2 bottomIndex)
    {
        BlockUVCoordinates blockUVCoord = new BlockUVCoordinates(topIndex, sideIndex, bottomIndex);
        MyBlockUvCoordinates.Add((int)(blockType), blockUVCoord);
    }


    #endregion
}
