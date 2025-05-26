using UnityEngine;

// Manages the tile grid for the game. Handles grid layout, access to cells, and helper functions.

public class TileGrid : MonoBehaviour
{
    // Array of rows in the grid
    public TileRow[] rows { get; private set; }

    // Flattened array of all tile cells in the grid
    public TileCell[] cells { get; private set; }

    // Total number of cells
    public int size => cells.Length;

    // Number of rows (grid height)
    public int height => rows.Length;

    // Number of columns (grid width)
    public int width => size / height;

    private void Awake()
    {
        // Cache all row and cell components in the grid at load time
        rows = GetComponentsInChildren<TileRow>();
        cells = GetComponentsInChildren<TileCell>();
    }

    private void Start()
    {
        // Assign grid coordinates (x, y) to each tile cell
        for (int y = 0; y < rows.Length; y++)
        {
            for (int x = 0; x < rows[y].cells.Length; x++)
            {
                rows[y].cells[x].coordinates = new Vector2Int(x, y);
            }
        }
    }
    public TileCell GetCell(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return rows[y].cells[x];
        }
        else
        {
            return null;
        }
    }

    public TileCell GetCell(Vector2Int coordinates)
    {
        return GetCell(coordinates.x, coordinates.y);
    }

    public TileCell GetAdjacentCell(TileCell cell, Vector2Int direction)
    {
        Vector2Int coordiantes = cell.coordinates;

        // X increases as you move right, Y decreases as you move up
        coordiantes.x += direction.x;
        coordiantes.y -= direction.y;

        return GetCell(coordiantes);
    }

    public TileCell GetRandomEmptyCell()
    {
        int index = Random.Range(0, cells.Length);
        int startingIndex = index;

        // Loop through cells until an unoccupied one is found
        while (cells[index].occupied)
        {
            index++;

            if (index >= cells.Length)
            {
                index = 0;
            }

            // If we looped all the way around, all cells are occupied
            if (index == startingIndex)
            {
                return null;
            }
        }

        return cells[index];
    }
}
