using UnityEngine;

public class Node_script
{
    public int                 F             { get; private set; }
    public int                 G             { get; private set; }
    public int                 H             { get; private set; }
    public Node_script         Parent        { get; private set; }
    public GridPosition_script Position      { get; private set; }
    public Tile_script         TileScript    { get; private set; }
    public Vector2             WorldPosition { get; private set; }

    public Node_script(Tile_script tileScript)
    {
        this.TileScript    = tileScript;
        this.Position      = tileScript.Position;
        this.WorldPosition = tileScript.WorldPosition;
    }

    public void CalculateCost(Node_script parent, Node_script goal, int gCost)
    {
        this.Parent = parent;
        this.G      = (parent.G + gCost);
        this.H      = (Mathf.Abs((this.Position.Column - goal.Position.Column) + (this.Position.Row - goal.Position.Row)) * 10);
        this.F      = (this.G + this.H);
    }
}
