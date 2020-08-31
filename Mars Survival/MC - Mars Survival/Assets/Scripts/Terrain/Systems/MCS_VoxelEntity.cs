using UnityEngine;

public class MCS_VoxelEntity
{
    private Vector3Int myWorldPos;
    private Vector3Int[] myPattern;

    public MCS_VoxelEntity(Vector3Int aWorldPos, Vector3Int[] aPattern)
    {
        myWorldPos = aWorldPos;
        myPattern = aPattern;
    }

    public MCS_VoxelEntity(Vector3Int aWorldPos, Entity aEntityType)
    {
        myWorldPos = aWorldPos;

        switch (aEntityType)
        {
            case Entity.Tree:
                myPattern = GetTreePattern();
                break;
        }
    }

    public Vector3Int[] GetTreePattern()
    {
        Vector3Int[] pattern = new Vector3Int[9];

        for (int i = 0; i < 5; i++)
        {
            pattern[i] = new Vector3Int(0, i, 0);
        }

        pattern[5] = new Vector3Int(-1, 3, 0);
        pattern[6] = new Vector3Int(0, 3, 1);
        pattern[7] = new Vector3Int(0, 3, -1);
        pattern[8] = new Vector3Int(1, 3, 0);

        return pattern;
    }

    public Vector3Int WorldPos
    {
        get { return myWorldPos; }
        set { myWorldPos = value; }
    }

    public Vector3Int[] Pattern
    {
        get { return myPattern; }
        set { myPattern = value; }
    }
}

public enum Entity
{
    Tree
}
