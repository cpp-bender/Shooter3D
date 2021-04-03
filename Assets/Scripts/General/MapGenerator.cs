using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Transform tilePrefab;
    [SerializeField] private Transform obstaclePrefab;
    [SerializeField] private Transform navMeshFloor;
    [SerializeField] private Transform navMeshMaskPrefab;
    [Range(0f, 1f)] [SerializeField] private float tileOffSet;
    [SerializeField] private Vector2 maxMapSize;
    [SerializeField] private float tileSize;
    [SerializeField] private Map map;

    private const string holderName = "Generated Map";
    private Transform mapHolder;
    private List<Coordinate> coordinates;
    private Queue<Coordinate> shuffledCoordinates;
    private BoxCollider mapCollider;

    private Queue<Coordinate> shuffledOpenTileCoordinates;
    private Transform[,] tileMap;

    private void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }
        InitializeComponents();
        GenerateTiles();
        GenerateObstacles();

        //Left NavMesh Mask
        Transform leftMask = Instantiate(navMeshMaskPrefab, Vector3.left * (maxMapSize.x + map.size.x) / 4f * tileSize, Quaternion.identity, mapHolder);
        leftMask.localScale = new Vector3((maxMapSize.x - map.size.x) / 2f, 1, map.size.y) * tileSize;

        //Right NavMesh Mask
        Transform rightMask = Instantiate(navMeshMaskPrefab, Vector3.right * (maxMapSize.x + map.size.x) / 4f * tileSize, Quaternion.identity, mapHolder);
        rightMask.localScale = new Vector3((maxMapSize.x - map.size.x) / 2f, 1, map.size.y) * tileSize;

        //Top NavMesh Mask
        Transform topMask = Instantiate(navMeshMaskPrefab, Vector3.forward * (maxMapSize.y + map.size.y) / 4f * tileSize, Quaternion.identity, mapHolder);
        topMask.localScale = new Vector3(maxMapSize.x, 1, (maxMapSize.y - map.size.y) / 2f) * tileSize;

        //Bottom NavMesh Mask
        Transform bottoMask = Instantiate(navMeshMaskPrefab, Vector3.back * (maxMapSize.y + map.size.y) / 4f * tileSize, Quaternion.identity, mapHolder);
        bottoMask.localScale = new Vector3(maxMapSize.x, 1, (maxMapSize.y - map.size.y) / 2f) * tileSize;

        navMeshFloor.localScale = new Vector3(maxMapSize.x, maxMapSize.y) * tileSize;
        mapCollider = GetComponent<BoxCollider>();
        mapCollider.size = new Vector3(map.size.x * tileSize, .5f, map.size.y * tileSize);
    }

    private void GenerateObstacles()
    {
        List<Coordinate> allOpenCoordinates = new List<Coordinate>(coordinates);
        System.Random rnd = new System.Random(map.seed);
        bool[,] obstacleMap = new bool[(int)map.size.x, (int)map.size.y];
        shuffledCoordinates = new Queue<Coordinate>(Utility.Shuffle(coordinates.ToArray(), map.seed));
        int obstacleCount = (int)(map.size.x * map.size.y * map.obstaclePercent);
        int currentObstacleCount = 0;
        for (int i = 0; i < obstacleCount; i++)
        {
            Coordinate randomCoordinate = GetRandomCoordinate();
            obstacleMap[randomCoordinate.x, randomCoordinate.y] = true;
            currentObstacleCount++;
            if (randomCoordinate != map.MapCentre && IsMapFullyAccessible(obstacleMap, currentObstacleCount))
            {
                float obstacleHeight = Mathf.Lerp(map.minObstacleHeight, map.maxObstacleHeight, (float)rnd.NextDouble());
                Vector3 obstaclePosition = CoordinateToPosition(randomCoordinate.x, randomCoordinate.y);
                var newobstacle = Instantiate(obstaclePrefab, obstaclePosition + (Vector3.up * obstacleHeight / 2f), Quaternion.identity);
                newobstacle.transform.parent = mapHolder;
                newobstacle.localScale = new Vector3(((1 - tileOffSet) * tileSize), obstacleHeight, (1 - tileOffSet) * tileSize);

                Renderer obstacleRenderer = newobstacle.GetComponent<Renderer>();
                Material obstacleMaterial = new Material(obstacleRenderer.sharedMaterial);
                float colourPercent = randomCoordinate.y / (float)map.size.y;
                obstacleMaterial.color = Color.Lerp(map.foregroundColor, map.backgroundColor, colourPercent);
                obstacleRenderer.sharedMaterial = obstacleMaterial;
                allOpenCoordinates.Remove(randomCoordinate);
            }
            else
            {
                obstacleMap[randomCoordinate.x, randomCoordinate.y] = false;
                currentObstacleCount--;
            }
        }
        allOpenCoordinates.Remove(map.MapCentre);
        shuffledOpenTileCoordinates = new Queue<Coordinate>(Utility.Shuffle(allOpenCoordinates.ToArray(), map.seed));
    }

    private void GenerateTiles()
    {
        coordinates = new List<Coordinate>();
        for (int x = 0; x < map.size.x; x++)
        {
            for (int y = 0; y < map.size.y; y++)
            {
                Vector3 tilePosition = CoordinateToPosition(x, y);
                var newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90), mapHolder.transform);
                newTile.localScale = Vector3.one * (1 - tileOffSet) * tileSize;
                AddCoordinates(x, y);
                tileMap[x, y] = newTile;
            }
        }
    }

    private bool IsMapFullyAccessible(bool[,] obstacleMap, int currentObstacleCount)
    {
        //The Flood-Fill Algorithm
        bool[,] mapFlags = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];
        Queue<Coordinate> coordinates = new Queue<Coordinate>();
        coordinates.Enqueue(map.MapCentre);
        mapFlags[map.MapCentre.x, map.MapCentre.y] = true;
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
        int targetAccessibleTileCount = (int)(map.size.x * map.size.y - currentObstacleCount);
        return targetAccessibleTileCount == accessibleTileCount;
    }

    private Vector3 CoordinateToPosition(int x, int y)
    {
        return new Vector3(-map.size.x / 2f + .5f + x, 0, -map.size.y / 2f + .5f + y) * tileSize;
    }

    private Coordinate GetRandomCoordinate()
    {
        Coordinate randomCoordinate = shuffledCoordinates.Dequeue();
        shuffledCoordinates.Enqueue(randomCoordinate);
        return randomCoordinate;
    }

    public Transform GetRandomOpenTile()
    {
        Coordinate randomOpenTileCoordinate = shuffledOpenTileCoordinates.Dequeue();
        shuffledOpenTileCoordinates.Enqueue(randomOpenTileCoordinate);
        return tileMap[randomOpenTileCoordinate.x, randomOpenTileCoordinate.y];
    }

    public Transform GetTileFromPosition(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x / tileSize + (map.size.x - 1) / 2f);
        int y = Mathf.RoundToInt(position.z / tileSize + (map.size.y - 1) / 2f);
        x = Mathf.Clamp(x, 0, tileMap.GetLength(0) - 1);
        y = Mathf.Clamp(y, 0, tileMap.GetLength(1) - 1);
        return tileMap[x, y];
    }

    private void InitializeComponents()
    {
        mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;
        tileMap = new Transform[map.size.x, map.size.y];
    }

    private void AddCoordinates(int x, int y)
    {
        coordinates.Add(new Coordinate(x, y));
    }
}

[System.Serializable]
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

[System.Serializable]
public class Map
{
    public Coordinate size;
    [Range(0, 1)] public float obstaclePercent;
    public int seed;
    public float minObstacleHeight;
    public float maxObstacleHeight;
    public Color backgroundColor;
    public Color foregroundColor;
    public Coordinate MapCentre
    {
        get
        {
            return new Coordinate(size.x / 2, size.y / 2);
        }
    }
}