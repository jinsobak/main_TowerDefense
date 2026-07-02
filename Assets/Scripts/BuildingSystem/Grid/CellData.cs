using UnityEngine;

public class CellData
{
    // x, y¿Œµ¶Ω∫
    public int x;
    public int y;
    // ∫Ûƒ≠ ø©∫Œ
    public bool isOccupied;

    public CellData(int x, int y)
    {
        this.x = x;
        this.y = y;
        isOccupied = false;
    }

    public CellData(Vector2Int index)
    {
        this.x = index.x;
        this.y = index.y;
        isOccupied = false;
    }
}
