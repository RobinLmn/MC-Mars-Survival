using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType : byte
{
    TopSoil = 0,
    Dirt = 1,
    Stone = 2,
    Air = 255

}

public enum Faces
{
    SIDE = 1,
    BOTTOM = 2,
    TOP = 0
}

public class MCS_Block : MC_Item
{
    public int myIndex;
    public BlockType Type;
    public byte LightAmount;
    public MCS_Chunk myChunk;
    public Vector3Int myWorldPos;
    public MCD_WorldData myWorldData;

    public void Dig()
    {
        SetType(BlockType.Air);
        myWorldData.MyChunkMesh.GenerateChunkMeshData(myChunk);
        myChunk.MyTransform.GetComponent<MCC_ChunkObject>().CreateChunkGameObjectMesh(myChunk);
    }

    public void Place(BlockType aBlockType)
    {
        SetType(aBlockType);
        myWorldData.MyChunkMesh.GenerateChunkMeshData(myChunk);
        myChunk.MyTransform.GetComponent<MCC_ChunkObject>().CreateChunkGameObjectMesh(myChunk);
    }

    public MCS_Block(BlockType type, Vector3Int aWorldPos, MCS_Chunk chunk)
    {
        isBlock = true;
        SetType(type);
        myWorldPos = aWorldPos;
        LightAmount = 1;
        myChunk = chunk;
    }

    public void SetType(BlockType type)
    {
        Type = type;

        switch (type)
        {
            case BlockType.Dirt:
                myItem = Item.Dirt;
                break;
            case BlockType.TopSoil:
                myItem = Item.Dirt;
                break;
            case BlockType.Stone:
                myItem = Item.Stone;
                break;
            case BlockType.Air:
                myItem = Item.Hand;
                break;
        }
    }
}

public class BlockUVCoordinates
{
    private readonly Vector2[] m_BlockFaceUvCoordinates = new Vector2[3];

    public BlockUVCoordinates(Vector2 topUvCoordinates, Vector2 sideUvCoordinates, Vector2 bottomUvCoordinates)
    {
        BlockFaceUvCoordinates[(int)Faces.TOP] = topUvCoordinates;
        BlockFaceUvCoordinates[(int)Faces.SIDE] = sideUvCoordinates;
        BlockFaceUvCoordinates[(int)Faces.BOTTOM] = bottomUvCoordinates;
    }

    public Vector2[] BlockFaceUvCoordinates
    {
        get { return m_BlockFaceUvCoordinates; }
    }
}