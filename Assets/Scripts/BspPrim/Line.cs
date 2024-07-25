using UnityEngine;
public class Line
{
    public Orientation orientation;
    public Vector2Int coord;

    public Line(Orientation orientation, Vector2Int coord)
    {
        this.orientation = orientation;
        this.coord = coord;
    }
}

public enum Orientation
{
    Horizontal = 0,
    Vertical = 1
}