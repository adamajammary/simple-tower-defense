//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

public struct GridPosition_script
{
    public int Row    { get; set; }
    public int Column { get; set; }

    public GridPosition_script(int row, int column)
    {
        this.Row    = row;
        this.Column = column;
    }

    public static bool operator ==(GridPosition_script pos1, GridPosition_script pos2)
    {
        return ((pos1.Row == pos2.Row) && (pos1.Column == pos2.Column));
    }

    public static bool operator !=(GridPosition_script pos1, GridPosition_script pos2)
    {
        return ((pos1.Row != pos2.Row) || (pos1.Column != pos2.Column));
    }

    public static GridPosition_script operator -(GridPosition_script pos1, GridPosition_script pos2)
    {
        return new GridPosition_script((pos1.Row - pos2.Row), (pos1.Column - pos2.Column));
    }

    public static GridPosition_script operator +(GridPosition_script pos1, GridPosition_script pos2)
    {
        return new GridPosition_script((pos1.Row + pos2.Row), (pos1.Column + pos2.Column));
    }
}
