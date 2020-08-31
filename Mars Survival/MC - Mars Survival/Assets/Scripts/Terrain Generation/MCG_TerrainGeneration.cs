using UnityEngine;

public class MCG_TerrainGeneration
{
    public void GenerateChunk(MCD_WorldData aWorldData, MCS_Chunk chunk)
    {
        for (int x = 0; x < aWorldData.MyChunkSizeX; x++)
        {
            int blocX = chunk.WorldPos.x + x;

            for (int z = 0; z < aWorldData.MyChunkSizeZ; z++)
            {
                int blocZ = chunk.WorldPos.z + z;
                float lowerGroundHeight = GetLowerGroundHeight(chunk, blocX, blocZ, x, z, aWorldData.MyTerrainDepth);
                int upperGround = GetUpperGroundHeight(aWorldData, blocX, blocZ, lowerGroundHeight);

                bool sunlit = true;
                for (int y = aWorldData.MyChunkSizeY - 1; y >= 0; y--)
                {
                    BlockType blockType;
                    if (y > upperGround)
                    {
                        blockType = BlockType.Air;
                    }
                    else if (y >= lowerGroundHeight)
                    {
                        if (sunlit)
                        {
                            blockType = BlockType.TopSoil;
                            // Remember, this adds the block global coordinates
                            chunk.TopSoilBlocks.Add(new Vector3Int(blocX, y, blocZ));
                            sunlit = false;
                        }
                        else
                        {
                            blockType = BlockType.Dirt;
                        }
                    }
                    else
                    {
                        blockType = BlockType.Stone;
                    }

                    Vector3Int pos = new Vector3Int(x, y, z);
                    aWorldData.GetBlock(pos + chunk.WorldPos).SetType(blockType);
                }
            }
        }
    }

    private static int GetUpperGroundHeight(MCD_WorldData aWorldData, int blockWorldX, int blockWorldZ,
                                        float lowerGroundHeight)
    {
        float octave1 = Mathf.PerlinNoise(blockWorldX * 0.001f, blockWorldZ * 0.001f) * 0.5f;
        float octave2 = Mathf.PerlinNoise((blockWorldX + 100) * 0.002f, blockWorldZ * 0.002f) * 0.25f;
        float octave3 = Mathf.PerlinNoise((blockWorldX + 100) * 0.01f, blockWorldZ * 0.01f) * 0.25f;
        float octaveSum = octave1 + octave2 + octave3;
        return (int)(octaveSum * 4 + (int)(lowerGroundHeight));
    }


    private static float GetLowerGroundHeight(MCS_Chunk chunk, int blockWorldX, int blockWorldZ, int blockXInChunk,
                                              int blockZInChunk, int worldDepthInBlocks)
    {
        int minimumGroundheight = worldDepthInBlocks / 4;
        int minimumGroundDepth = (int)(worldDepthInBlocks * 0.5f);

        float octave1 = Mathf.PerlinNoise(blockWorldX * 0.0001f, blockWorldZ * 0.0001f) * 0.5f;
        float octave2 = Mathf.PerlinNoise(blockWorldX * 0.0005f, blockWorldZ * 0.0005f) * 0.35f;
        float octave3 = Mathf.PerlinNoise(blockWorldX * 0.02f, blockWorldZ * 0.02f) * 0.15f;
        float lowerGroundHeight = octave1 + octave2 + octave3;
        lowerGroundHeight = lowerGroundHeight * minimumGroundDepth + minimumGroundheight;

        return lowerGroundHeight;
    }
}