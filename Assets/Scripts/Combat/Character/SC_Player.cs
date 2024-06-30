using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class SC_Player : SC_Character
{
    public bool AllowMove {get; set;}

    public override void Initialize(SC_Tile startTile)
    {
        base.Initialize(startTile);
    }

    protected override void Update()
    {
        base.Update();
        ProcessMovement();
    }

    private void ProcessMovement()
    {
        if (!AllowMove) {return;}

        if (Input.GetKeyDown(KeyCode.W))
        {
            Move(Vector2Int.up);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Move(Vector2Int.down);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Move(Vector2Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Move(Vector2Int.right);
        }
    }

    protected override void InitializeStates()
    {
        turnState.TurnStart = new PlayerTurnStart(this);
        turnState.Action = new PlayerAction(this);
        turnState.TurnEnd = new PlayerTurnEnd(this);
    }
}

public class PlayerTurnStart : CharacterTurnStart
{
    private SC_Player player;
    public PlayerTurnStart(SC_Player character) : base(character)
    {
        player = character;
    }

    public override void Enter()
    {
        player.AllowMove = true;
        base.Enter();
    }
}


public class PlayerAction : CharacterAction
{
    private SC_Player player;
    private bool turnEndBtnPressed;

    public PlayerAction(SC_Player character) : base(character)
    {
        player = character;
        turnEndBtnPressed = false;
    }

    public override void Enter()
    {
        base.Enter();

        SC_BattleManager.Instance.TurnEndBtn += () => turnEndBtnPressed = true;
    }

    public override IEnumerator Update()
    {
        yield return base.Update();

        Debug.Log($"{player.name} is waiting for turn end button");
        yield return new WaitUntil(() => turnEndBtnPressed);
    }

    public override void Exit()
    {
        base.Exit();

        SC_BattleManager.Instance.TurnEndBtn -= () => turnEndBtnPressed = true;
    }
}

public class PlayerTurnEnd : CharacterTurnEnd
{
    private SC_Player player;
    public PlayerTurnEnd(SC_Player character) : base(character)
    {
        player = character;
    }

    public override void Enter()
    {
        base.Enter();

        player.AllowMove = false;
    }
}