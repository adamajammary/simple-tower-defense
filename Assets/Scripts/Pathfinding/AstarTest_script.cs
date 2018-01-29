//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarTest_script : MonoBehaviour
{
    private Tile_script _goal;
    private Tile_script _start;

    [SerializeField]
    private GameObject _arrowPrefab;

    private Stack<Node_script> _path;
    private List<GameObject>   _arrows;

    private void Start()
    {
        this._arrows = new List<GameObject>();
        this._path   = new Stack<Node_script>();
    }

    private void Update()
    {
        this.CheckRightClickTile();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if ((this._start != null) && (this._goal != null)) {
                this._path = Astar_script.GetPath(this._start.Position, this._goal.Position);
                this.Visualize(_path, Color.blue);
            }
        }
    }

    private void CheckRightClickTile()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit2D mouseHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (mouseHit.collider != null)
            {
                Tile_script tileHit = mouseHit.collider.GetComponent<Tile_script>();

                if (tileHit != null) {
                    if (this._start == null) {
                        this._start                      = tileHit;
                        this._start.Debugging            = true;
                        this._start.SpriteRenderer.color = Color.green;
                    } else if (this._goal == null) {
                        this._goal                      = tileHit;
                        this._goal.Debugging            = true;
                        this._goal.SpriteRenderer.color = Color.red;
                    } else {
                        this.ResetVisualization();
                    }
                }
            }
        }
    }

    private void ResetVisualization()
    {
        this._start.SpriteRenderer.color = Color.white;
        this._goal.SpriteRenderer.color  = Color.white;
        this._start                      = null;
        this._goal                       = null;
        this.Visualize(_path, Color.white);
        this._path.Clear();

        foreach (GameObject arrow in this._arrows) {
            Destroy(arrow);
        }
    }

    private void VisualizeParent(Node_script node, Vector2 worldPosition)
    {
        if (node.Parent != null)
        {
            GameObject arrow = Instantiate(this._arrowPrefab, worldPosition, Quaternion.identity);

            arrow.transform.SetParent(this.transform);

            // RIGHT
            if ((node.Position.Column < node.Parent.Position.Column) && (node.Position.Row == node.Parent.Position.Row)) {
                arrow.transform.eulerAngles = new Vector3(0, 0, 0);
            // RIGHT-DOWN
            } else if ((node.Position.Column < node.Parent.Position.Column) && (node.Position.Row < node.Parent.Position.Row)) {
                arrow.transform.eulerAngles = new Vector3(0, 0, -45);
            // RIGHT-UP
            } else if ((node.Position.Column < node.Parent.Position.Column) && (node.Position.Row > node.Parent.Position.Row)) {
                arrow.transform.eulerAngles = new Vector3(0, 0, 45);
            // LEFT
            } else if ((node.Position.Column > node.Parent.Position.Column) && (node.Position.Row == node.Parent.Position.Row)) {
                arrow.transform.eulerAngles = new Vector3(0, 0, 180);
            // LEFT-DOWN
            } else if ((node.Position.Column > node.Parent.Position.Column) && (node.Position.Row < node.Parent.Position.Row)) {
                arrow.transform.eulerAngles = new Vector3(0, 0, -135);
            // LEFT-UP
            } else if ((node.Position.Column > node.Parent.Position.Column) && (node.Position.Row > node.Parent.Position.Row)) {
                arrow.transform.eulerAngles = new Vector3(0, 0, 135);
            // DOWN
            } else if ((node.Position.Column == node.Parent.Position.Column) && (node.Position.Row < node.Parent.Position.Row)) {
                arrow.transform.eulerAngles = new Vector3(0, 0, -90);
            // UP
            } else if ((node.Position.Column == node.Parent.Position.Column) && (node.Position.Row > node.Parent.Position.Row)) {
                arrow.transform.eulerAngles = new Vector3(0, 0, 90);
            }

            _arrows.Add(arrow);
        }
    }

    public void Visualize(Node_script node, Color color)
    {
        node.TileScript.SpriteRenderer.color = color;
    }

    public void Visualize(HashSet<Node_script> nodes, Color color)
    {
        foreach (Node_script node in nodes) {
            if ((node.TileScript != this._start) && (node.TileScript != this._goal)) {
                node.TileScript.Debugging            = true;
                node.TileScript.SpriteRenderer.color = color;
            }

            this.VisualizeParent(node, node.TileScript.WorldPosition);
        }
    }

    public void Visualize(Stack<Node_script> path, Color color)
    {
        foreach (Node_script node in path) {
            if ((node.TileScript != this._start) && (node.TileScript != this._goal)) {
                node.TileScript.Debugging            = true;
                node.TileScript.SpriteRenderer.color = color;
            }

            this.VisualizeParent(node, node.TileScript.WorldPosition);
        }
    }

    
}
