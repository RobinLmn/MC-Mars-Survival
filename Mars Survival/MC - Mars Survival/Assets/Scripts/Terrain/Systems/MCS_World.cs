using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCS_World
{
    private MCD_WorldData myWorldData;

    public MCS_World(MCD_WorldData aWorldData)
    {
        myWorldData = aWorldData;
    }

    public void InitChunkGrid()
    {
        for (int x = 0; x < myWorldData.MyTerrainSizeX / myWorldData.MyChunkSizeX; x++)
        {
            for (int y = 0; y < myWorldData.MyTerrainSizeY / myWorldData.MyChunkSizeY; y++)
            {
                for (int z = 0; z < myWorldData.MyTerrainSizeZ / myWorldData.MyChunkSizeZ; z++)
                {
                    Vector3Int chunkPos = new Vector3Int(x, y, z);
                    MCS_Chunk newChunk = new MCS_Chunk(myWorldData, chunkPos);
                    myWorldData.MyChunks[x, y, z] = newChunk;
                    myWorldData.MyChunks[x, y, z].GenerateBlocks();
                }
            }
        }
    }
}
