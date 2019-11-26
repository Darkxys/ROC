using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinding
{

    public static List<Vector2Int> FindPath(Vector2Int start, Vector2Int end, int[,] map)
    {

        List<Cell> cells = new List<Cell>();
        List<Cell> visitedCells = new List<Cell>();

        int DistanceFromEnd = (int)Vector2.Distance(start, end);
        cells.Add(new Cell(start, 0, DistanceFromEnd, null));

        Cell lastCell = null;
        int attempt = 0;
        while (attempt < DistanceFromEnd * 100)
        {
            attempt++;

            //Order cells by their F.
            List<Cell> unvisitedCells = cells.FindAll(cell => !cell.visited);
            unvisitedCells.OrderBy(cell => cell.f);

            //Order cells by their H instead if multiple cells have the same lower F value.
            List<Cell> cellsWithLowF = unvisitedCells.FindAll(cell => cell.f == unvisitedCells[0].f);
            if (cellsWithLowF.Count > 1)
            {
                unvisitedCells.OrderBy(cell => cell.h);
            }

            if (unvisitedCells.Count == 0) return new List<Vector2Int>();

            Cell currentCell = unvisitedCells[0];

            if (currentCell.pos == end)
            {
                lastCell = currentCell;
                break;
            }

            //Current cell has now been visited.
            currentCell.visited = true;

            //Update cells next to the current one.
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {

                    if (i == 0 && j == 0) continue; //Skip the current cell;

                    Vector2Int newPos = currentCell.pos + new Vector2Int(i, j);

                    if (newPos.x < 0 || newPos.x >= map.GetLength(0) || newPos.y < 0 || newPos.y >= map.GetLength(1)) continue;
                    if (map[newPos.x, newPos.y] == 1) continue;

                    Cell newCell = new Cell(newPos, (int)(currentCell.g + Vector2.Distance(currentCell.pos, newPos)), (int)Vector2.Distance(newPos, end), currentCell);

                    int existingCellIndex = cells.FindIndex(cell => cell.pos == newPos);

                    //Add the new cell in the list or replace the old one.
                    if (existingCellIndex == -1)
                    {
                        cells.Add(newCell);
                    }
                    else
                    {
                        Cell existingCell = cells[existingCellIndex];
                        if (!existingCell.visited && existingCell.f > newCell.f)
                        {
                            existingCell = newCell;
                        }
                    }

                }
            }

        }

        List<Vector2Int> finalPath = new List<Vector2Int>();

        if (lastCell != null)
        {
            Cell curCell = lastCell;
            while (curCell.pos != start)
            {
                finalPath.Add(curCell.pos);
                curCell = curCell.parent;
            }
        }

        return finalPath;

    }

}
