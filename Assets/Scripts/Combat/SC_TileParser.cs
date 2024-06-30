using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFSW.QC;

public class SC_TileParser : MonoBehaviour
{
    public int MaxX = 8;
    public int MaxY = 4;

    public GameObject TileParent;
    private SC_Tile[,] TileMatrix;

    #region Singleton
    public static SC_TileParser Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private void Start()
    {
        TileMatrix = new SC_Tile[MaxX, MaxY];

        List<SC_Tile> tiles = new List<SC_Tile>();
        foreach (Transform child in TileParent.transform)
        {
            tiles.Add(child.GetComponent<SC_Tile>());
        }
        ParseTiles(tiles);
    }

    private void ParseTiles(List<SC_Tile> tiles)
    {
        tiles.Sort((a, b) =>
        {
            Vector3 aPos = a.transform.position;
            Vector3 bPos = b.transform.position;
            if (Mathf.Abs(aPos.x - bPos.x) > 0.1f)
            {
                return aPos.x.CompareTo(bPos.x);
            }
            else
            {
                return aPos.z.CompareTo(bPos.z);
            }
        });

        for (int i = 0; i < tiles.Count; i++)
        {
            int y = i % 4;
            int x = i / 4;

            var tile = tiles[i].GetComponent<SC_Tile>();
            TileMatrix[x, y] = tile;

            bool inEnemySide = x >= 4;
            tile.Init(x, y, inEnemySide);
        }
    }

    // 以 4*8 矩阵为基础的取值
    [Command("GetTile", MonoTargetType.Single)]
    public SC_Tile GetTile(int x, int y)
    {
        if (x >= MaxX || y >= MaxY)
        {
            Debug.LogError("Invalid tile position");
            return null;
        }

        return TileMatrix[x, y];
    }

    // 以两个矩阵（玩家左敌人右）为基础的取值
    [Command("GetTile", MonoTargetType.Single)]
    public SC_Tile GetTile(int x, int y, bool inEnemySide){
        if (x >= MaxX / 2 || y >= MaxY)
        {
            Debug.LogError($"Invalid tile position: {x}, {y}, {inEnemySide}");
            return null;
        }

        if(inEnemySide){
            x += MaxX / 2;
        }

        return TileMatrix[x, y];
    }

    public SC_Tile GetTile(TilePosition tilePosition){
        return GetTile(tilePosition.X, tilePosition.Y, tilePosition.InEnemySide);
    }

    [ContextMenu("DebugPrintMatrix")]
    public void DebugPrintMatrix()
    {
        for (int z = 0; z < MaxY; z++)
        {
            string row = "";
            for (int x = 0; x < MaxX; x++)
            {
                row += TileMatrix[x, z].Position.X + "," + TileMatrix[x, z].Position.Y + " ";
            }
            Debug.Log(row);
        }
    }
}
