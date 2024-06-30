using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class TilePosition
{
    [SerializeField] private int x;
    [SerializeField] private int y;
    [ReadOnly] private bool inEnemySide;

    public int X { get { return x; } protected set { x = value; } }
    public int Y { get { return y; } protected set { y = value; } }
    public bool InEnemySide { get { return inEnemySide; } protected set { inEnemySide = value; } }

    // 我在考虑把它作为纯判断区块的tag，而不影响coordinate本身
    public TilePosition(int x, int y, bool inEnemySide)
    {
        X = x;
        Y = y;
        InEnemySide = inEnemySide;
    }

    public override bool Equals(object obj)
    {
        var a = obj as TilePosition;
        return a != null && a.X == X && a.Y == Y && a.InEnemySide == InEnemySide;
    }
    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}
