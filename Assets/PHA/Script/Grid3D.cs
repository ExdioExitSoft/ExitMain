using UnityEngine;

public class Grid3D : MonoBehaviour
{
    public static int width = 10;
    public static int height = 20;
    public static int depth = 10;

    public static Transform[,,] grid = new Transform[width, height, depth];

    public static Vector3 Round(Vector3 pos)
    {
        return new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));
    }

    public static bool InsideGrid(Vector3 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < width &&
                (int)pos.y >= 0 && (int)pos.y < height &&
                (int)pos.z >= 0 && (int)pos.z < depth);
    }

    public static Transform GetTransformAtGridPosition(Vector3 pos)
    {
        if (pos.y >= height)
            return null;

        return grid[(int)pos.x, (int)pos.y, (int)pos.z];
    }

    public static void AddBlockToGrid(Transform block)
    {
        foreach (Transform child in block)
        {
            Vector3 pos = Round(child.position);
            grid[(int)pos.x, (int)pos.y, (int)pos.z] = child;
        }
    }

    public static bool IsLineFull(int y)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                if (grid[x, y, z] == null)
                    return false;
            }
        }
        return true;
    }

    public static void DeleteLine(int y)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                Destroy(grid[x, y, z].gameObject);
                grid[x, y, z] = null;
            }
        }
    }

    public static void MoveAllBlocksDown(int y)
    {
        for (int i = y; i < height - 1; i++)
        {
            MoveLineDown(i);
        }
    }

    public static void MoveLineDown(int y)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                if (grid[x, y, z] != null)
                {
                    grid[x, y - 1, z] = grid[x, y, z];
                    grid[x, y, z] = null;
                    grid[x, y - 1, z].position += Vector3.down;
                }
            }
        }
    }

    public static void DeleteFullLines()
    {
        for (int y = 0; y < height; y++)
        {
            if (IsLineFull(y))
            {
                DeleteLine(y);
                MoveAllBlocksDown(y);
                y--;
            }
        }
    }
}