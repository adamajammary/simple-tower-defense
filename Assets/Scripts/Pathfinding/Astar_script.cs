using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Astar_script
{
    private static Dictionary<GridPosition_script, Node_script> _nodes;

    public static void CreateNodes()
    {
        _nodes = new Dictionary<GridPosition_script, Node_script>();

        foreach (Tile_script tile in TileMap_script.Instance.Tiles.Values) {
            _nodes.Add(tile.Position, new Node_script(tile));
        }
    }

    public static Stack<Node_script> GetPath(GridPosition_script startPosition, GridPosition_script goalPosition)
    {
        if (_nodes == null) {
            CreateNodes();
        }

        int                  gCost;
        Node_script          neighbor;
        GridPosition_script  neighborPosition;
        Node_script          currentNode = _nodes[startPosition];
        Node_script          goalNode    = _nodes[goalPosition];
        HashSet<Node_script> openList    = new HashSet<Node_script>();
        HashSet<Node_script> closedList  = new HashSet<Node_script>();
        Stack<Node_script>   path        = new Stack<Node_script>();

        openList.Add(currentNode);

        while (openList.Count > 0)
        {
            // GET THE NODE WITH THE LOWEST F COST
            currentNode = openList.OrderBy(n => n.F).First();

            // GOAL REACHED
            if (currentNode == goalNode)
            {
                // RECONSTRUCT PATH BY BACKTRACING PARENTS
                path.Push(currentNode);

                while (currentNode.Parent != null) {
                    currentNode = currentNode.Parent;
                    path.Push(currentNode);
                }

                break;
            }

            // COSE CURRENT
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // OPEN NEIGHBORS
            for (int row    = -1; row    <= 1; row++) {
            for (int column = -1; column <= 1; column++)
            {
                neighborPosition = new GridPosition_script(currentNode.Position.Row - row, currentNode.Position.Column - column);

                // SKIP OUT-OF-BOUNDS, UNWALKABLES AND CURRENT NODE
                if (TileMap_script.Instance.IsWithinBounds(neighborPosition) &&
                    (TileMap_script.Instance.Tiles[neighborPosition].IsWalkable) && (neighborPosition != currentNode.Position))
                {
                    neighbor = _nodes[neighborPosition];
                
                    // SKIP CLOSED NODE
                    if (closedList.Contains(neighbor)) {
                        continue;
                    }

                    // HORIZONTAL/VERTICAL
                    if (Mathf.Abs(column - row) == 1) {
                        gCost = 10;
                    // DIAGONAL
                    } else {
                        if (!IsMoveAllowed(currentNode, neighbor)) {
                            continue;
                        }
                        gCost = 14;
                    }

                    // NEW/UNVISITED
                    if (!openList.Contains(neighbor)) {
                        openList.Add(neighbor);
                    // SKIP IF NOT BETTER
                    } else if ((currentNode.G + gCost) >= neighbor.G) {
                        continue;
                    }

                    // UPDATE COST
                    neighbor.CalculateCost(currentNode, goalNode, gCost);
                }
            }}
        }

        _nodes.Clear();
        _nodes = null;

        //// VIZUALIZE OPEN LIST
        //GameObject.Find("AstarTest_object").GetComponent<AstarTest_script>().Visualize(openList,   Color.yellow);
        //GameObject.Find("AstarTest_object").GetComponent<AstarTest_script>().Visualize(closedList, Color.cyan);
        //GameObject.Find("AstarTest_object").GetComponent<AstarTest_script>().Visualize(path,       Color.blue);

        return path;
    }

    private static bool IsMoveAllowed(Node_script currentNode, Node_script neighbor)
    {
        GridPosition_script direction = (neighbor.Position - currentNode.Position);
        GridPosition_script first     = new GridPosition_script(currentNode.Position.Row, (currentNode.Position.Column + direction.Column));
        GridPosition_script second    = new GridPosition_script((currentNode.Position.Row + direction.Row), currentNode.Position.Column);

        if (TileMap_script.Instance.IsWithinBounds(first) && !TileMap_script.Instance.Tiles[first].IsWalkable) {
            return false;
        }

        if (TileMap_script.Instance.IsWithinBounds(second) && !TileMap_script.Instance.Tiles[second].IsWalkable) {
            return false;
        }

        return true;
    }
}
