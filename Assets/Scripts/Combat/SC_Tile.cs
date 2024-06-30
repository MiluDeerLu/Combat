using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SC_Tile : MonoBehaviour
{
    private static SC_TileParser gridParser { get { return SC_TileParser.Instance; } }

    [ShowInInspector]
    public SC_Character Character { get; private set; }
    public TilePosition Position { get; private set; }

    public void Init(int x, int y, bool inEnemySide)
    {
        Position = new TilePosition(x, y, inEnemySide);
    }

    public void SetCharacter(SC_Character character)
    {
        Character = character;
    }

    public bool Occupied()
    {
        return Character != null;
    }

    // if out of world range return null
    public SC_Tile GetWorldRelative(Vector2Int direction)
    {
        int x = Position.X + direction.x;
        int y = Position.Y + direction.y;
        if (IsWithinGlobalArea(x, y))
        {
            return gridParser.GetTile(x, y);
        }
        return null;
    }

    // if out of own section's range return null
    public SC_Tile GetSectionRelative(Vector2Int direction)
    {
        int x = Position.X + direction.x;
        int y = Position.Y + direction.y;
        if (IsWithinSectionArea(x, y, Position.InEnemySide))
        {
            return gridParser.GetTile(x, y);
        }
        return null;
    }


    private bool IsWithinGlobalArea(int x, int y)
    {
        return x >= 0 && x < gridParser.MaxX && y >= 0 && y < gridParser.MaxY;
    }

    private bool IsWithinSectionArea(int x, int y, bool inEnemySide)
    {
        if (!(y >= 0 && y < gridParser.MaxY))
        {
            return false;
        }

        if (!inEnemySide)
        {
            return x >= 0 && x < gridParser.MaxX / 2;
        }
        else
        {
            return x >= gridParser.MaxX / 2 && x < gridParser.MaxX;
        }
    }
}
