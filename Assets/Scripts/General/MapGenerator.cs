using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Transform tilePrefab;
    [SerializeField] private int obstacleCount;
    [SerializeField] private Transform obstaclePrefab;
    [SerializeField] private Vector2 mapSize;
    [Range(0f, 1f)] [SerializeField] private float tileOffSet;

    private const string holderName = "Generated Map";
    private Transform mapHolder;

    List<Coordinate> coordinates;
    Queue<Coordinate> shuffledCoordinates;

    public void GenerateMap()
    {
        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }
        InitializeMapHolder();
        GenerateTiles();
        GenerateObstacles();
    }

    private void GenerateObstacles()
    {
        shuffledCoordinates = new Queue<Coordinate>(Utility.Shuffle(coordinates.ToArray(), 1));
        for (int i = 0; i < obstacleCount; i++)
        {
            Coordinate randomCoordinate = GetRandomCoordinate();
            Vector3 obstaclePosition = CoordinateToPosition(randomCoordinate.x, randomCoordinate.y);
            var newobstacle = Instantiate(obstaclePrefab, obstaclePosition + Vector3.up * .5f, Quaternion.identity);
            newobstacle.transform.parent = mapHolder;
            newobstacle.localScale = Vector3.one * (1 - tileOffSet);
        }
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
}