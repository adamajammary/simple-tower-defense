//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap_script : Singleton<TileMap_script>
{
    [SerializeField]
    private GameObject[] _tilePrefabs;

    [SerializeField]
    private Camera_script _cameraObject;

    [SerializeField]
    private GameObject _portalStartPrefab;

    [SerializeField]
    private GameObject _portalEndPrefab;

    private GridPosition_script _mapSize;
    private Stack<Node_script>  _monsterPath;
    private GridPosition_script _portalStartSpawnPos, _portalEndSpawnPos;

    private Vector3 TileSize
    {
        get { return this._tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size; }
    }

    public GridPosition_script                          PortalStartSpawnPos { get { return this._portalStartSpawnPos; } }
    public Portal_script                                PortalStart         { get; private set; }
    public Dictionary<GridPosition_script, Tile_script> Tiles               { get; private set; }

    public Stack<Node_script> MonsterPath
    {
        get {
            if (this._monsterPath == null) { this.GeneratePath(); }
            return new Stack<Node_script>(new Stack<Node_script>(this._monsterPath));
        }
    }

    private void Start()
    {
        this.Tiles = new Dictionary<GridPosition_script, Tile_script>();
        this.LoadTiles();
    }

    private void Update()
    {
        //
	}

    public void GeneratePath()
    {
        this._monsterPath = Astar_script.GetPath(this._portalStartSpawnPos, this._portalEndSpawnPos);
    }

    public bool IsWithinBounds(GridPosition_script position)
    {
        return ((position.Column >= 0) && (position.Row >= 0) && (position.Column < this._mapSize.Column) && (position.Row < this._mapSize.Row));
    }

    private int[][] LoadFromFile(string file)
    {
        List<int[]> tiles     = new List<int[]>();
        TextAsset   textFile  = (TextAsset)Resources.Load(file);
        string[]    tilesTemp = textFile.text.Split(System.Environment.NewLine.ToCharArray());
        string[]    tilesTemp2;

        foreach (string line in tilesTemp)
        {
            if (line.Length > 0)
            {
                tilesTemp2 = line.Split(';');

                List<int> row = new List<int>();
                foreach (string word in tilesTemp2) {
                    row.Add(int.Parse(word));
                }
                tiles.Add(row.ToArray());
            }
        }

        return tiles.ToArray();
    }

    private void LoadTiles()
    {
        Tile_script lastTile;
        Vector2     lastTilePosition;
        int[][]     mapData = this.LoadFromFile("Levels/level_001_resource");
        Vector3     offset  = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));

        this._mapSize = new GridPosition_script(mapData.Length, mapData[0].Length);

        for (int row    = 0; row    < this._mapSize.Row;    row++)    {
        for (int column = 0; column < this._mapSize.Column; column++) {
            this.PlaceTile(mapData[row][column], row, column, offset);
        }}

        lastTile         = this.Tiles[new GridPosition_script((this._mapSize.Row - 1), (this._mapSize.Column - 1))];
        lastTilePosition = lastTile.transform.position;

        this._cameraObject.SetLimit(new Vector3(lastTilePosition.x + this.TileSize.x, lastTilePosition.y - this.TileSize.y, 0));

        this.SpawnPortals();
    }

    private void PlaceTile(int type, int row, int column, Vector3 offset)
    {
        Tile_script tile = Instantiate(_tilePrefabs[type]).GetComponent<Tile_script>();

        tile.Initialize(
            new GridPosition_script(row, column),
            new Vector3((offset.x + (this.TileSize.x * column)), (offset.y - (this.TileSize.y * row)), 0),
            this.transform,
            type
        );
    }

    private void SpawnPortals()
    {
        this._portalStartSpawnPos = new GridPosition_script(0, 0);
        this._portalEndSpawnPos   = new GridPosition_script(6, 16);


        GameObject portalStart = Instantiate(
            this._portalStartPrefab, this.Tiles[this._portalStartSpawnPos].GetComponent<Tile_script>().WorldPosition, Quaternion.identity
        );

        this.PortalStart      = portalStart.GetComponent<Portal_script>();
        this.PortalStart.name = "portal_002_prefab";

        GameObject portalEnd = Instantiate(
            this._portalEndPrefab, this.Tiles[this._portalEndSpawnPos].GetComponent<Tile_script>().WorldPosition, Quaternion.identity
        );

        portalStart.transform.SetParent(GameObject.Find("Portals_object").transform);
        portalEnd.transform.SetParent(GameObject.Find("Portals_object").transform);
    }
}
