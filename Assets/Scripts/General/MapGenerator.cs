using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Transform tilePrefab;
    [SerializeField] private Transform obstaclePrefab;
    [SerializeField] private Vector2 mapSize;
    [Range(0f, 1f)] [SerializeField] private float tileOffSet;
    [Range(0f, 1f)] [SerializeField] private float obstaclePercent;
    [SerializeField] private int seed;

    private const string holderName = "Generated Map";
    private Transform mapHolder;
    private Coordinate center;
    private List<Coordinate> coordinates;
    private Queue<Coordinate> shuffledCoordinates;

    public void GenerateMap()
    {
        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }
        center = new Coordinate((int)mapSize.x / 2, (int)mapSize.y / 2);
        InitializeMapHolder();
        GenerateTiles();
        GenerateObstacles();
    }

    private void GenerateObstacles()
    {
        bool[,] obstacleMap = new bool[(int)mapSize.x, (int)mapSize.y];
        shuffledCoordinates = new Queue<Coordinate>(Utility.Shuffle(coordinates.ToArray(), seed));
        int obstacleCount = (int)(mapSize.x * mapSize.y * obstaclePercent);
        int currentObstacleCount = 0;
        for (int i = 0; i < obstacleCount; i++)
        {
            Coordinate randomCoordinate = GetRandomCoordinate();
            obstacleMap[randomCoordinate.x, randomCoordinate.y] = true;
            currentObstacleCount++;
            if (randomCoordinate != center && IsMapFullyAccessible(obstacleMap, currentObstacleCount))
            {
                Vector3 obstaclePosition = CoordinateToPosition(randomCoordinate.x, randomCoordinate.y);
                var newobstacle = Instantiate(obstaclePrefab, obstaclePosition + Vector3.up * .5f, Quaternion.identity);
                newobstacle.transform.parent = mapHolder;
                newobstacle.localScale = Vector3.one * (1 - tileOffSet);
            }
            else
            {
                obstacleMap[randomCoordinate.x, randomCoordinate.y] = false;
                currentObstacleCount--;
            }
        }
    }

    private bool IsMapFullyAccessible(bool[,] obstacleMap, int currentObstacleCount)
    {
        //The Flood-Fill Algorithm
        bool[,] mapFlags = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];
        Queue<Coordinate> coordinates = new Queue<Coordinate>();
        coordinates.Enqueue(center);
        mapFlags[center.x, center.y] = true;
        int accessibleTileCount = 1;
        while (coordinates.Count > 0)
        {
            Coordinate tile = coordinates.Dequeue();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int neighbourX = tile.x + x;
                    int neighbourY = tile.y + y;
                    if (x == 0 | y == 0)
                    {
                        if ((neighbourX >= 0 && neighbourX < obstacleMap.GetLength(0)) && neighbourY >= 0 && neighbourY < obstacleMap.GetLength(1))
                        {
                            if (!mapFlags[neighbourX, neighbourY] && !obstacleMap[neighbourX, neighbourY])
                            {
                                mapFlags[neighbourX, neighbourY] = true;
                                coordinates.Enqueue(new Coordinate(neighbourX, neighbourY));
                                accessibleTileCount++;
                            }
                        }
                    }
                }
            }
        }
        int targetAccessibleTileCount = (int)(mapSize.x * mapSize.y - currentObstacleCount);
        return targetAccessibleTileCount == accessibleTileCount;
    }

    private Vector3 CoordinateToPosition(int x, int y)
    {
        return new Vector3(-mapSize.x / 2 + .5f + x, 0, -mapSize.y / 2 + .5f + y);
    }

    private Coordinate GetRandomCoordinate()
    {
        Coordinate coordinate = shuffledCoordinates.Dequeue();
        shuffledCoordinates.Enqueue(coordinate);
        return coordinate;
    }

    private void GenerateTiles()
    {
        coordinates = new List<Coordinate>();
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector3 tilePosition = CoordinateToPosition(x, y);
                var newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90), mapHolder.transform);
                newTile.localScale = Vector3.one * (1 - tileOffSet);
                AddCoordinates(x, y);
            }
        }
    }

    private void InitializeMapHolder()
    {
        mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;
    }

    private void AddCoordinates(int x, int y)
    {
        coordinates.Add(new Coordinate(x, y));
    }
}

public struct Coordinate
{
    public int x;
    public int y;
    public Coordinate(int _x, int _y)
    {
        x = _x;
        y = _y;
    }
    public static bool operator ==(Coordinate c1, Coordinate c2)
    {
        return c1.x == c2.x && c1.y == c2.y;
    }
    public static bool operator !=(Coordinate c1, Coordinate c2)
    {
        return !(c1 == c2);
    }
}