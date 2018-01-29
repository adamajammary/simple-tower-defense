//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile_script : MonoBehaviour
{

    private Color32        _colorAvailable;
    private Color32        _colorOccupied;
    private SpriteRenderer _spriteRenderer;
    private Tower_script   _tower;

    public bool                Debugging      { get; set; }
    public bool                IsAvailable    { get; set; }
    public bool                IsWalkable     { get; private set; }
    public GridPosition_script Position       { get; private set; }
    public SpriteRenderer      SpriteRenderer { get { return this._spriteRenderer; } }

    public Vector2 WorldPosition
    {
        get {
            return new Vector2(
                this.transform.position.x + (GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2),
                this.transform.position.y - (GetComponent<SpriteRenderer>().sprite.bounds.size.y / 2)
            );
        }
    }


    private void Start()
    {
        this._colorAvailable = new Color32(0x60, 0xFF, 0x5A, 0xFF);
        this._colorOccupied  = new Color32(0xFF, 0x76, 0x76, 0xFF);
        this._spriteRenderer = GetComponent<SpriteRenderer>();
        this.Debugging       = false;
    }

    private void Update()
    {
		//
	}

    private void OnMouseExit()
    {
        if (!this.Debugging) {
            this.SetColor(Color.white);
        }
    }

    private void OnMouseOver()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Game_script.Instance.ClickedButton != null)
            {
                if (this.IsAvailable && !this.Debugging) {
                    this.SetColor(this._colorAvailable);

                    if (Input.GetMouseButtonDown(0)) {
                        this.PlaceTower();
                    }
                } else if (!this.Debugging) {
                    this.SetColor(this._colorOccupied);
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                if (this._tower != null) {
                    Game_script.Instance.SelectTower(this._tower);
                } else {
                    Game_script.Instance.DeselectTower();
                }
            }
        }
    }

    public void Initialize(GridPosition_script position, Vector3 worldPosition, Transform parent, int type)
    {
        this.IsAvailable        = (type == 0);
        this.IsWalkable         = (type > 0);
        this.Position           = position;
        this.transform.position = worldPosition;

        this.transform.SetParent(parent);

        TileMap_script.Instance.Tiles.Add(new GridPosition_script(this.Position.Row, this.Position.Column), this);
    }

    private void PlaceTower()
    {
        GameObject tower = (GameObject)Instantiate(Game_script.Instance.ClickedButton.TowerPrefab, this.WorldPosition, Quaternion.identity);

        tower.GetComponent<SpriteRenderer>().sortingOrder = this.Position.Row;

        tower.transform.SetParent(this.transform);

        this._tower = tower.transform.GetChild(0).GetComponent<Tower_script>();

        this.IsAvailable = false;
        this.IsWalkable  = false;

        this._tower.Price = Game_script.Instance.ClickedButton.Price;

        Game_script.Instance.PurchaseTower();
    }

    private void SetColor(Color32 color)
    {
        this._spriteRenderer.color = color;
    }
}
