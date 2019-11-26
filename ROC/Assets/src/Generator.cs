using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Text;
enum TileType
{
    Wall,
    Path,
    Room
}

public class Generator
{
    #region Properties

    private int _sizeX;
    private int _sizeY;
    private int _widthPath = 5;
    private int _numberAttemptRoom = 100;
    private System.Random _rnd = new System.Random(System.Environment.TickCount);
    private int _columns;
    private TileType[,] _maze;
    private List<Vector2Int> _directions;

    private Dictionary<Rect, Salle> _listRooms;
    private List<Salle> _rooms;
    private List<Salle> _importantRoom;
    private List<Vector2> _lstPath;
    private int _quantityRoom;

    #endregion


    /// <summary>
    /// The main function to produce a dungeon
    /// </summary>

    public Generator(int width, int height, TypeDonjon type, int quantity)
    {
        _importantRoom = new List<Salle>();
        _rooms = new List<Salle>();

        _sizeX = width;
        _sizeY = height;
        _quantityRoom = quantity;

        _importantRoom.Add(type.lstSalle[0]);
        _importantRoom.Add(type.lstSalle[1]);

        foreach (Salle salle in type.lstSalle)
        {
            if (!_importantRoom.Contains(salle))
                _rooms.Add(salle);
        }
    }

    #region Methods to generate the dungeon
    public void Generate()
    {
        Init();
        InitMaze();

        while (_listRooms.Count < 2)
            GenerateRooms();
        GenerateMaze();

        CarveDoor();

        RemoveDeadEnd();
    }

    public int[,] GenerateGround()
    {
        int[,] tlmGround = new int[_sizeX, _sizeY];

        for (int x = 0; x < _maze.GetLength(0); x++)
        {
            for (int y = 0; y < _maze.GetLength(1); y++)
            {
                if (_maze[x, y] != TileType.Wall) tlmGround[x, y] = 0;
                else tlmGround[x, y] = 1;
            }
        }

        foreach (KeyValuePair<Rect, Salle> aRoom in _listRooms)
        {
            for (int yPosition = 0; yPosition < aRoom.Value.size; yPosition++)
            {
                for (int xPosition = 0; xPosition < aRoom.Value.size; xPosition++)
                {
                    int x = xPosition + (int)aRoom.Key.x;
                    int y = yPosition + (int)aRoom.Key.y;
                    tlmGround[x, y] = aRoom.Value.layerFloor[yPosition * aRoom.Value.size + xPosition];
                }
            }
        }

        return tlmGround;
    }

    public int[,] GenerateRoomCollision()
    {
        int[,] tlm = new int[_sizeX, _sizeY];

        foreach (KeyValuePair<Rect, Salle> aRoom in _listRooms)
        {
            for (int yPosition = 0; yPosition < aRoom.Value.size; yPosition++)
            {
                for (int xPosition = 0; xPosition < aRoom.Value.size; xPosition++)
                {
                    int x = xPosition + (int)aRoom.Key.x;
                    int y = yPosition + (int)aRoom.Key.y;
                    tlm[x, y] = aRoom.Value.layerCollision[yPosition * aRoom.Value.size + xPosition];
                }
            }
        }

        return tlm;
    }

    public int[,] GenerateCollision()
    {
        int[,] tlmCollision = new int[_sizeX, _sizeY];

        for (int x = 0; x < _maze.GetLength(0); x++)
        {
            for (int y = 0; y < _maze.GetLength(1); y++)
            {
                if (_maze[x, y] == TileType.Wall) tlmCollision[x, y] = GetWallIndex(x, y);
                else tlmCollision[x, y] = 0;
            }
        }

        return tlmCollision;
    }

    private int GetWallIndex(int x, int y)
    {
        string walls = "";

        for (int i = 1; i >= -1; i--)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (x + j < 0 || x + j >= _maze.GetLength(0) || y + i < 0 || y + i >= _maze.GetLength(1)) walls += "1";
                else if (_maze[x + j, y + i] == TileType.Wall) walls += "1";
                else walls += "0";
            }
        }

        switch (walls)
        {
            case "111111110": return 1;
            case "111111011": return 3;
            case "110111111": return 7;
            case "011111111": return 9;
            case "000011011": return 10;
            case "000110110": return 11;
            case "011011000": return 12;
            case "110110000": return 13;
            default:
                break;
        }

        if (walls[7] == '1' && walls[1] == '0') return 8;
        else if (walls[7] == '0' && walls[1] == '1') return 2;
        else if (walls[5] == '1' && walls[3] == '0') return 6;
        else if (walls[5] == '0' && walls[3] == '1') return 4;

        return 5;
    }

    public int[,] GenerateAir()
    {
        int[,] tlmAir = new int[_sizeX, _sizeY];

        foreach (KeyValuePair<Rect, Salle> aRoom in _listRooms)
        {
            for (int xPosition = (int)aRoom.Key.x; xPosition < aRoom.Key.width + (int)aRoom.Key.x; xPosition++)
            {
                for (int yPosition = (int)aRoom.Key.y; yPosition < aRoom.Key.height + (int)aRoom.Key.y; yPosition++)
                {
                    tlmAir[xPosition, yPosition] = aRoom.Value.layerAir[(xPosition - (int)aRoom.Key.x) * (int)aRoom.Key.width + (yPosition - (int)aRoom.Key.y)];
                }
            }
        }

        return tlmAir;
    }

    public Vector3 GetPosStart()
    {
        Vector2 posRoomStart = Vector2.one;
        int sizeRoom = 25;

        foreach (KeyValuePair<Rect, Salle> aRoom in _listRooms)
        {
            if (aRoom.Value == _importantRoom[0])
            {
                posRoomStart = aRoom.Key.position;
                sizeRoom = aRoom.Value.size;
                break;
            }
        }

        Vector3 posPlayer = new Vector3(posRoomStart.x + (sizeRoom / 2) + 1, posRoomStart.y + (sizeRoom / 2) + 1, 0);

        return posPlayer;
    }

    public Vector3 GetChestPos()
    {
        Vector3 posChest = Vector3.one;

        /*
         Steps:

         1 - Pick a random room (not an important one)
         2 - Random x and y between 0 and 2 included
         3 - Check if collision
         4 - if not, return that pos, else continue
         
         */



        while (posChest == Vector3.one)
        {
            int rndIndex = _rnd.Next(2, _listRooms.Count - 1);
            int compteur = 0;
            KeyValuePair<Rect, Salle> aRoom;

            foreach (KeyValuePair<Rect, Salle> room in _listRooms)
            {
                if (compteur == rndIndex)
                {
                    aRoom = room;
                    break;
                }

                compteur++;
            }

            int rndX = _rnd.Next(2);
            int rndY = _rnd.Next(2);

            int xPos = (rndX * (aRoom.Value.size / 2));
            int yPos = (rndY * (aRoom.Value.size / 2));

            if (aRoom.Value.layerCollision[xPos + yPos * aRoom.Value.size] == 0)
            {
                if (rndX == 0) xPos += 1;
                if (rndY == 0) yPos += 1;
                if (rndX == 2) xPos -= 1;
                if (rndY == 2) xPos -= 1;

                Vector3 pos = new Vector3(xPos + aRoom.Key.x, yPos + aRoom.Key.y, 0);

                return pos;
            }

        }

        return posChest;
    }

    public Vector3 GetPosBoss()
    {
        Vector2 posRoomStart = Vector2.one;
        int sizeRoom = 45;

        foreach (KeyValuePair<Rect, Salle> aRoom in _listRooms)
        {
            if (aRoom.Value == _importantRoom[1])
            {
                posRoomStart = aRoom.Key.position;
                sizeRoom = aRoom.Value.size;
                break;
            }
        }

        Vector3 posPlayer = new Vector3(posRoomStart.x + (sizeRoom / 2) + 1, posRoomStart.y + (sizeRoom / 2) + 1, 0);

        return posPlayer;
    }

    public List<Vector3> GetPointsOfSpawn()
    {
        List<Vector3> points = new List<Vector3>();

        /*
            Steps:
         1 - Get points for room (not the first one)
         2 - Get points for path (with _lstPath)
         3 - return
         
         */
        int maximumGroupsPourcent = 70;

        foreach (KeyValuePair<Rect, Salle> aRoom in _listRooms)
        {
            if (aRoom.Value == _importantRoom[0] || aRoom.Value == _importantRoom[1]) continue;

            int counter = 0;
            int numberOfPoints = (aRoom.Value.size - 1) / 4;
            Vector2Int posStart = new Vector2Int(2, 2);
            Vector2Int posCurrent = new Vector2Int(posStart.x, posStart.y);

            for (int x = 0; x < numberOfPoints; x++)
            {
                for (int y = 0; y < numberOfPoints; y++)
                {
                    int rndChance = _rnd.Next(10);

                    if (rndChance < 1)
                    {
                        points.Add(new Vector3(posCurrent.x + aRoom.Key.x, posCurrent.y + aRoom.Key.y, 0));
                        counter++;

                        if (counter == (numberOfPoints * numberOfPoints * maximumGroupsPourcent) / 100)
                            break;
                    }

                    posCurrent.y += 4;
                }
                posCurrent.y = posStart.y;
                posCurrent.x += 4;
            }

        }

        foreach (Vector2 point in _lstPath)
        {
            int rndChance = _rnd.Next(9);

            if (rndChance == 7)
                points.Add(new Vector3(point.x, point.y, 0));
        }

        return points;
    }

    private void Init()
    {
        _maze = new TileType[_sizeX, _sizeY];
        _listRooms = new Dictionary<Rect, Salle>();
        _columns = (_widthPath - 1) / 2;
        _lstPath = new List<Vector2>();
        InitDirections();
    }

    private void InitDirections()
    {
        _directions = new List<Vector2Int>();

        _directions.Add(new Vector2Int(0, 1));
        _directions.Add(new Vector2Int(0, -1));
        _directions.Add(new Vector2Int(1, 0));
        _directions.Add(new Vector2Int(-1, 0));
    }

    private void InitMaze()
    {
        // Init the maze with all walls 
        for (int x = 0; x < _sizeX; x++)
        {
            for (int y = 0; y < _sizeY; y++)
            {
                _maze[x, y] = TileType.Wall;
            }
        }
    }

    private void GenerateRooms()
    {
        int attemps = _numberAttemptRoom;
        while (attemps > 0 && _listRooms.Count < _quantityRoom)
        {
            Salle roomToAdd;

            int rndRoomIndex = 0;

            if (attemps >= _numberAttemptRoom - 1) roomToAdd = _importantRoom[_numberAttemptRoom - attemps];
            else roomToAdd = _rooms[rndRoomIndex];

            Rect room = Overlaps(roomToAdd);

            // If it doesn't, add it to the list of rooms
            if (room != Rect.zero) _listRooms.Add(room, roomToAdd);
            else if (room == Rect.zero && attemps >= _numberAttemptRoom - 1)
            {
                int counter = 0;
                while (room == Rect.zero && counter < 20)
                {
                    room = Overlaps(roomToAdd);
                    counter++;
                }
                if (counter >= 20)
                {
                    _listRooms = new Dictionary<Rect, Salle>();
                    return;
                }
                _listRooms.Add(room, roomToAdd);
            }

            attemps--;
        }

        foreach (KeyValuePair<Rect, Salle> aRoom in _listRooms)
        {
            for (int xPosition = (int)aRoom.Key.x; xPosition < aRoom.Key.width + (int)aRoom.Key.x; xPosition++)
            {
                for (int yPosition = (int)aRoom.Key.y; yPosition < aRoom.Key.height + (int)aRoom.Key.y; yPosition++)
                {
                    _maze[xPosition, yPosition] = TileType.Room;
                }
            }
        }
    }

    private Rect Overlaps(Salle roomToAdd)
    {
        // Generate a random rectangle with _sizeRooms X and Y
        float xRect = _rnd.Next(_sizeX - roomToAdd.size);
        while (xRect % 2 == 0 || xRect % 5 != _columns - 1 || xRect < roomToAdd.size) xRect = _rnd.Next(_sizeX);
        float yRect = _rnd.Next(_sizeY - roomToAdd.size);
        while (yRect % 2 == 0 || yRect % 5 != _columns - 1 || yRect < roomToAdd.size) yRect = _rnd.Next(_sizeY);

        if (!(xRect + roomToAdd.size - 1 >= _maze.GetLength(0) - roomToAdd.size || yRect + roomToAdd.size - 1 >= _maze.GetLength(1) - roomToAdd.size))
        {

            Rect room = new Rect(xRect, yRect, roomToAdd.size, roomToAdd.size);

            foreach (KeyValuePair<Rect, Salle> aRoom in _listRooms)
            {
                Vector2Int vectorPos = new Vector2Int((int)aRoom.Key.x - _widthPath * 2, (int)aRoom.Key.y - _widthPath * 2);

                Rect roomOverlapsZone = new Rect(vectorPos, new Vector2(aRoom.Key.width + _widthPath * 4, aRoom.Key.height + _widthPath * 4));
                if (room.Overlaps(roomOverlapsZone))
                    return Rect.zero;
            }

            return room;
        }

        return Rect.zero;
    }

    private void GenerateMaze()
    {
        // Begin to generate the maze by picking a cell with odd position
        for (int xPosition = (1 + _columns); xPosition < _sizeX; xPosition += (2 * _widthPath))
        {
            for (int yPosition = (1 + _columns); yPosition < _sizeY; yPosition += (2 * _widthPath))
            {
                if (_maze[xPosition, yPosition] == TileType.Wall) GeneratePath(xPosition, yPosition);
            }
        }
    }

    private void GeneratePath(int xPosition, int yPosition)
    {
        List<Vector2Int> cellsCarved = new List<Vector2Int>();

        Vector2Int position = new Vector2Int(xPosition, yPosition);

        Carve(position);

        cellsCarved.Add(position);
        _lstPath.Add(position);

        int compteur = 0;

        while (cellsCarved.Count > 0 && compteur < 10000)
        {
            Vector2Int cell = cellsCarved[cellsCarved.Count - 1];

            RandomizeDirList();

            bool carved = false;
            foreach (Vector2Int direction in _directions)
            {
                Vector2Int positionEnd = (direction * 2 * _widthPath) + cell;

                if (IsAbleToCarve(positionEnd) && _maze[positionEnd.x, positionEnd.y] == TileType.Wall)
                {

                    carved = true;

                    Carve((direction * _widthPath) + cell);
                    Carve(positionEnd);

                    cellsCarved.Add(positionEnd);
                    _lstPath.Add((direction * _widthPath) + cell);
                    _lstPath.Add(positionEnd);
                    break;

                }
            }

            compteur++;

            if (!carved)
                cellsCarved.Remove(cell);
        }
    }

    private void Carve(Vector2Int posToCarve)
    {
        int posCornerX = posToCarve.x - _columns;
        int posCornerY = posToCarve.y - _columns;

        for (int xPos = 0; xPos < _widthPath; xPos++)
        {
            for (int yPos = 0; yPos < _widthPath; yPos++)
            {
                _maze[xPos + posCornerX, yPos + posCornerY] = TileType.Path;
            }
        }
    }

    private bool IsAbleToCarve(Vector2Int end)
    {
        if (end.y < _columns || end.y >= _maze.GetLength(1) - _columns) return false;
        if (end.x < _columns || end.x >= _maze.GetLength(0) - _columns) return false;

        int xPos = end.x - _columns;
        int yPos = end.y - _columns;

        for (int x = 0; x < _widthPath - 1; x++)
        {
            for (int y = 0; y < _widthPath - 1; y++)
            {
                if (yPos + y >= _maze.GetLength(1) || xPos + x >= _maze.GetLength(0)) return false;
                if (_maze[x + xPos, y + yPos] != TileType.Wall) return false;
            }
        }

        return true;
    }

    private void CarveDoor()
    {
        foreach (KeyValuePair<Rect, Salle> aRoom in _listRooms)
        {
            List<Vector2Int> bounds = new List<Vector2Int>();

            for (int x = _columns; x < aRoom.Key.width - _columns; x += _widthPath * 2)
                if (!CheckAccessible(x + (int)aRoom.Key.x, (int)aRoom.Key.y - _columns - 1, Vector2Int.up, ref bounds)) continue;
            for (int x = _columns; x < aRoom.Key.width - _columns; x += _widthPath * 2)
                if (!CheckAccessible(x + (int)aRoom.Key.x, (int)aRoom.Key.y + (int)aRoom.Key.height + _columns, Vector2Int.down, ref bounds)) continue;
            for (int y = _columns; y < aRoom.Key.height - _columns; y += _widthPath * 2)
                if (!CheckAccessible((int)aRoom.Key.x - _columns - 1, y + (int)aRoom.Key.y, Vector2Int.left, ref bounds)) continue;
            for (int y = _columns; y < aRoom.Key.height - _columns; y += _widthPath * 2)
                if (!CheckAccessible((int)aRoom.Key.x + (int)aRoom.Key.width + _columns, y + (int)aRoom.Key.y, Vector2Int.right, ref bounds)) continue;

            int randomNumber = _rnd.Next(bounds.Count - 1);

            Vector2Int positionToCarve = bounds[randomNumber];
            Carve(positionToCarve);
            bounds.RemoveAt(randomNumber);

            int randomNumberExtra = _rnd.Next(20);

            if (randomNumberExtra <= 7 && bounds.Count > 3)
            {
                randomNumber = _rnd.Next(bounds.Count - 1);
                Vector2Int positionToCarveExtra = bounds[randomNumber];

                Carve(positionToCarveExtra);
            }

        }
    }

    private bool IsNear(Vector2Int pos1, Vector2Int pos2)
    {
        if (pos1.x + _widthPath == pos2.x || pos1.x - _widthPath == pos2.x) return true;
        if (pos1.y + _widthPath == pos2.y || pos1.y - _widthPath == pos2.y) return true;
        return false;
    }

    private bool CheckAccessible(int x, int y, Vector2Int dir, ref List<Vector2Int> bounds)
    {
        if (x < 0 || y < 0 || x >= _maze.GetLength(0) || y >= _maze.GetLength(1)) return false;

        Vector2Int pos = new Vector2Int(x, y) + (dir * _widthPath);

        if (pos.x < 0 || pos.y < 0 || pos.x >= _maze.GetLength(0) || pos.y >= _maze.GetLength(1)) return false;
        if (_maze[pos.x, pos.y] == TileType.Wall) return false;

        bounds.Add(new Vector2Int(x, y));

        return true;
    }

    private void RemoveDeadEnd()
    {
        bool done = false;

        while (!done)
        {
            done = true;

            for (int x = 1 + _columns; x < _sizeX; x += _widthPath)
            {
                for (int y = 1 + _columns; y < _sizeY; y += _widthPath)
                {
                    if (_maze[x, y] == TileType.Wall) continue;

                    int exits = 0;
                    foreach (Vector2Int positionOffset in _directions)
                    {
                        Vector2Int pos = new Vector2Int(x, y) + positionOffset * _widthPath;

                        if (pos.x < 0 || pos.x >= _maze.GetLength(0) || pos.y < 0 || pos.y >= _maze.GetLength(1)) continue;

                        if (_maze[pos.x, pos.y] != TileType.Wall) exits++;
                    }

                    if (exits != 1) continue;

                    done = false;
                    Uncarve(x, y);

                    _lstPath.RemoveAll((Vector2 v) =>
                    {
                        return ((x == (int)v.x) && (y == (int)v.y));
                    });
                }
            }
        }
    }

    private void Uncarve(int x, int y)
    {
        int posCornerX = x - _columns;
        int posCornerY = y - _columns;

        for (int xPos = 0; xPos < _widthPath; xPos++)
        {
            for (int yPos = 0; yPos < _widthPath; yPos++)
            {
                _maze[xPos + posCornerX, yPos + posCornerY] = TileType.Wall;
            }
        }
    }

    /// <summary>
    /// Method to mix all different direction (N,E,S,W)
    /// </summary>
    private void RandomizeDirList()
    {
        for (int i = 0; i < _directions.Count; i++)
        {
            int randomNumber = _rnd.Next(_directions.Count);
            Vector2Int tmp = _directions[i];
            _directions[i] = _directions[randomNumber];
            _directions[randomNumber] = tmp;
        }
    }

    public int CountRooms() { return _listRooms.Count; }
    #endregion
}
