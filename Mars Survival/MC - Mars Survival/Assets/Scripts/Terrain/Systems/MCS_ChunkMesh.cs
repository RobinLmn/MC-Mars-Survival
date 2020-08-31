using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCS_ChunkMesh
{
    private MCD_WorldData myWorldData;

    public MCS_ChunkMesh(MCD_WorldData aWorldData)
    {
        myWorldData = aWorldData;
    }

    public void GenerateChunkMeshData(MCS_Chunk chunk)
    {
        int index = 0;
        chunk.Vertices = new List<Vector3>();
        chunk.Indices = new List<int>();
        chunk.Uvs = new List<Vector2>();
        chunk.Colors = new List<Color>();


        int chunkX = chunk.WorldPos.x;
        int chunkY = chunk.WorldPos.y;
        int chunkZ = chunk.WorldPos.z;

        for (int x = 0; x < myWorldData.MyChunkSizeX; x++)
        {
            int blockX = chunkX + x;
            for (int y = 0; y < myWorldData.MyChunkSizeY; y++)
            {
                int blockY = chunkY + y;
                for (int z = 0; z < myWorldData.MyChunkSizeZ; z++)
                {
                    int blockZ = chunkZ + z;
                    Vector3Int pos = new Vector3Int(blockX, blockY, blockZ);
                    MCS_Block block = myWorldData.GetBlock(pos);
                    block.myIndex = index;
                    index = CreateDataMeshForBlock(block);
                }
            }
        }
    }

    public int CreateDataMeshForBlock(MCS_Block aBlock)
    {
        int index = aBlock.myIndex;
        MCS_Chunk chunk = aBlock.myChunk;

        int blockX = aBlock.myWorldPos.x;
        int blockY = aBlock.myWorldPos.y;
        int blockZ = aBlock.myWorldPos.z;

        int x = blockX - aBlock.myChunk.WorldPos.x;
        int y = blockY - aBlock.myChunk.WorldPos.y;
        int z = blockZ - aBlock.myChunk.WorldPos.z;
            
        MCS_Block currentBlock = aBlock;
        
        byte lightAmount = currentBlock.LightAmount;

        if (currentBlock.Type != BlockType.Air)
        {
            return index;
        }

        MCS_Block blockBelow = myWorldData.GetBlock(blockX, blockY -1, blockZ);
        if (blockBelow != null && blockBelow.Type != BlockType.Air)
        {
            // If bloc below is solid, we add a below face facing up
            AddBlockSide(x + 1, y, z,
                         x, y, z,
                         x, y, z + 1,
                         x + 1, y, z + 1,
                         0.5f, chunk, index, blockBelow.Type, Faces.BOTTOM, lightAmount);
            index += 4;
        }

        MCS_Block blockWest= myWorldData.GetBlock(blockX - 1, blockY, blockZ);
        if (blockWest != null && blockWest.Type != BlockType.Air)
        {
            // if bloc west (left) is solid, then render its west side facing east
            AddBlockSide(x, y, z,
                         x, y + 1, z,
                         x, y + 1, z + 1,
                         x, y, z + 1,
                         0.8f, chunk, index, blockWest.Type, Faces.SIDE, lightAmount);
            index += 4;
        }

        MCS_Block blockTop = myWorldData.GetBlock(blockX, blockY +1, blockZ);
        if (blockTop != null && blockTop.Type != BlockType.Air)
        {
            AddBlockSide(x + 1, y + 1, z,
                         x + 1, y + 1, z + 1,
                         x, y + 1, z + 1,
                         x, y + 1, z,               
                         0.9f, chunk, index, blockTop.Type, Faces.TOP, lightAmount);

            index += 4;
        }

        MCS_Block blockEast = myWorldData.GetBlock(blockX + 1, blockY, blockZ);
        if (blockEast != null && blockEast.Type != BlockType.Air)
        {
            AddBlockSide(x + 1, y, z,
                         x + 1, y, z + 1,
                         x + 1, y + 1, z + 1,
                         x + 1, y + 1, z,
                          0.7f, chunk, index, blockEast.Type, Faces.SIDE, lightAmount);

            index += 4;
        }
         
        MCS_Block blockNorth = myWorldData.GetBlock(blockX, blockY, blockZ + 1);
        if (blockNorth != null && blockNorth.Type != BlockType.Air)
        {
            AddBlockSide(x, y, z + 1,
                         x, y + 1, z + 1,
                         x + 1, y + 1, z + 1,
                         x + 1, y, z + 1,   
                         0.4f, chunk, index, blockNorth.Type, Faces.SIDE, lightAmount);

            index += 4;
        }

        MCS_Block blockSouth = myWorldData.GetBlock(blockX, blockY, blockZ - 1);
        if (blockSouth != null && blockSouth.Type != BlockType.Air)
        {
            AddBlockSide(x, y, z,
                         x + 1, y, z,
                         x + 1, y + 1, z,
                         x, y + 1, z,
                         1.0f, chunk, index, blockSouth.Type, Faces.SIDE, lightAmount);

            index += 4;
        }

        return index;
    }

    private void AddBlockSide(int x1, int y1, int z1, int x2, int y2, int z2, int x3, int y3, int z3, int x4,
                          int y4, int z4, float color, MCS_Chunk chunk, int index, BlockType blockType,
                          Faces blockFace,
                          byte blockLight)
    {
        // Add a block's side, add the information to the mesh lists for the chunk.

        float actualColor = 1f;
        const float epsilon = 0.02f;
        chunk.Vertices.Add(new Vector3(x1, y1, z1));
        chunk.Vertices.Add(new Vector3(x2, y2, z2));
        chunk.Vertices.Add(new Vector3(x3, y3, z3));
        chunk.Vertices.Add(new Vector3(x4, y4, z4));

        var item = new Color(actualColor, actualColor, actualColor);
        chunk.Colors.Add(item);
        chunk.Colors.Add(item);
        chunk.Colors.Add(item);
        chunk.Colors.Add(item);

        chunk.Indices.Add(index);
        chunk.Indices.Add(index + 1);
        chunk.Indices.Add(index + 2);

        chunk.Indices.Add(index + 2);
        chunk.Indices.Add(index + 3);
        chunk.Indices.Add(index);

        Vector2 textureIndex = myWorldData.MyBlockUvCoordinates[(int)blockType].BlockFaceUvCoordinates[(int)blockFace];

        float indexX = 0.5f * textureIndex.x;
        float indexY = 0.5f * textureIndex.y;
        float width = 0.48f;

        chunk.Uvs.Add(new Vector2(indexX + epsilon, indexY + epsilon));
        chunk.Uvs.Add(new Vector2(indexX + epsilon, indexY + width - epsilon));
        chunk.Uvs.Add(new Vector2(indexX + width - epsilon, indexY + width - epsilon));
        chunk.Uvs.Add(new Vector2(indexX + width - epsilon, indexY + epsilon)); 
    }
}
