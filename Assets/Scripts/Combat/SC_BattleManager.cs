using System;
using System.Collections.Generic;
using System.Linq;
using QFSW.QC;
using UnityEngine;
public class SC_BattleManager : MonoBehaviour
{
    # region Private Members
    private SC_TileParser tileParser;
    private SC_UIManager uiManager;

    private LinkedList<SC_Character> turnQueue = new LinkedList<SC_Character>();
    private LinkedListNode<SC_Character> currentTurnNode;
    private SC_Character unitInAction;
    private uint turnNum = 0;

    #endregion

    # region Public Members For Editor
    public SO_BattleInitData battleInitData;
    public Transform CharacterParent;
    # endregion

    # region Public Members Exposed To Scripts
    public List<SC_Character> players { get; private set; } = new List<SC_Character>();
    public List<SC_Character> enemies { get; private set; } = new List<SC_Character>();
    // 只用于方便fetch角色，不用于回合管理
    public List<SC_Character> allCharacters
    {
        get
        {
            return new List<SC_Character>(players.Concat(enemies));
        }
    }
    # endregion

    # region Singleton
    public static SC_BattleManager Instance { get; private set; }

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
    # endregion

    public event Action TurnEndBtn;

    private void Start()
    {
        tileParser = GetComponent<SC_TileParser>();
        uiManager = GetComponent<SC_UIManager>();
        InitBattle();
    }

    private void InitBattle()
    {
        foreach (var characterData in battleInitData.InitData)
        {
            var tile = tileParser.GetTile(characterData.Coordinate.X, characterData.Coordinate.Y);
            var character = Instantiate(characterData.Prefab).GetComponent<SC_Character>();
            character.Initialize(tile);
            character.transform.position = tile.transform.position;
            character.transform.SetParent(CharacterParent);

            if (character is SC_Player)
            {
                players.Add(character);
            }
            else
            {
                enemies.Add(character);
            }

            turnQueue.AddLast(character);
        }

        SortCharactersBySpeed();

        DebugPrintQueue();

        uiManager.InitializeQueue(turnQueue);

        currentTurnNode = turnQueue.First;
        unitInAction = currentTurnNode.Value;
        unitInAction.StartTurn();
    }

    public void TurnEndBtnPressed()
    {
        TurnEndBtn.Invoke();
    }

    public void NextTurn()
    {
        ChooseNextUnit();
        uiManager.UpdateQueue(turnQueue, currentTurnNode);
    }

    void SortCharactersBySpeed()
    {
        // 降序排列 -- 速度快的在前面
        turnQueue = new LinkedList<SC_Character>(turnQueue.OrderByDescending(character => character.characterData.Speed));
    }

    void ChooseNextUnit()
    {
        if (currentTurnNode == null || currentTurnNode.Next == null)
        {
            currentTurnNode = turnQueue.First;
            turnNum++;
        }
        else
        {
            currentTurnNode = currentTurnNode.Next;
        }

        unitInAction = currentTurnNode.Value;
        unitInAction.StartTurn();
    }

    public void RemoveCharacter(SC_Character character)
    {
        turnQueue.Remove(character);
        if (character is SC_Player)
        {
            players.Remove(character);
        }
        else
        {
            enemies.Remove(character);
        }
        uiManager.UpdateQueue(turnQueue, currentTurnNode);
    }

    // 默认生在当前unit后
    public void SpawnCharacter(GameObject prefab, SC_Tile tile)
    {
        var character = Instantiate(prefab).GetComponent<SC_Character>();
        character.Initialize(tile);
        character.transform.position = tile.transform.position;
        character.transform.SetParent(CharacterParent);

        if (character is SC_Player)
        {
            players.Add(character);
        }
        else
        {
            enemies.Add(character);
        }

        turnQueue.AddLast(character);
        uiManager.UpdateQueue(turnQueue, currentTurnNode);
    }

    // 或者可以选择生在特定角色后（比如机械战警被动）-- 具体会不会有bug得后续测试
    public void SpawnCharacter(GameObject prefab, SC_Tile tile, SC_Character spawnAfterCharacter)
    {
        var character = Instantiate(prefab).GetComponent<SC_Character>();
        character.Initialize(tile);
        character.transform.position = tile.transform.position;
        character.transform.SetParent(CharacterParent);

        if (character is SC_Player)
        {
            players.Add(character);
        }
        else
        {
            enemies.Add(character);
        }

        turnQueue.AddAfter(turnQueue.Find(spawnAfterCharacter), character);
        uiManager.UpdateQueue(turnQueue, currentTurnNode);
    }

    # region DebugUtilities
    [Command("DebugPrintQueue", MonoTargetType.Single)]
    public void DebugPrintQueue()
    {
        string queue = "";
        foreach (var character in turnQueue)
        {
            queue += character.name + " ";
        }
        Debug.Log(queue);
    }

    [Command("DebugPrintCurrentTurn", MonoTargetType.Single)]
    public void DebugPrintCurrentTurn()
    {
        Debug.Log(currentTurnNode.Value.name);
    }
    # endregion
}