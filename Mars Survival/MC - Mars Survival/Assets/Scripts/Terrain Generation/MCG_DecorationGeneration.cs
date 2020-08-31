using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCG_DecorationGeneration
{
    MCD_WorldData myWorldData;

    public MCG_DecorationGeneration(MCD_WorldData aWorldData)
    {
        myWorldData = aWorldData;
    }
    
    public void GenerateDecoration( MCS_Chunk aChunk)
    {
        Vector3Int offset = new Vector3Int(0, 1, 0);
        //SpawnDecoration(aChunk, Entity.Tree, aChunk.TopSoilBlocks[38] + offset);
    }

    public void SpawnDecoration(MCS_Chunk aChunk, Entity aEntityType, Vector3Int aWorldPos)
    {
        MCS_VoxelEntity entity = new MCS_VoxelEntity(aWorldPos, aEntityType);

        for (int i=0; i< entity.Pattern.Length; i++)
        {
            Vector3Int pos = entity.WorldPos + entity.Pattern[i];
            MCS_Block block = myWorldData.GetBlock(pos);

            block.Type = BlockType.Stone;
        }
    }
}