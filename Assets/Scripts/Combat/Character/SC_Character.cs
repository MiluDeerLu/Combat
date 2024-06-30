using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFSW.QC;
using Sirenix.OdinInspector;
using Mono.CSharp;


public abstract class SC_Character : MonoBehaviour
{
    [SerializeField] private SO_CharacterData SOCharacterData;
    public CharacterData characterData { get; private set;}

    [SerializeField] protected SC_Tile currentTile;
    private SC_Movement movement;
    public float BaseMoveSpeed = 1.0f;

    protected CharacterStateMachine turnState;

    public virtual void Initialize(SC_Tile startTile)
    {
        characterData = new CharacterData(SOCharacterData);
        movement = new SC_Movement(this, BaseMoveSpeed);

        currentTile = startTile;
        currentTile.SetCharacter(this);
        transform.position = startTile.transform.position;
    }

    protected virtual void Update()
    {
    }

    protected abstract void InitializeStates();

    public void StartTurn()
    {
        turnState = new CharacterStateMachine(this);
        InitializeStates();
        turnState.StartTurn(turnState.TurnStart);
    }

    public void SetTile(SC_Tile tile)
    {
        currentTile = tile;
    }

    public SC_Tile GetTile()
    {
        return currentTile;
    }

    [Command("GetPlayerPosition", MonoTargetType.Argument)]
    public TilePosition GetPosition()
    {
        return currentTile.Position;
    }

    public void Move(Vector2Int direction)
    {
        StartCoroutine(IEMove(direction));
    }

    protected IEnumerator IEMove(Vector2Int direction)
    {
        var targetTile = currentTile.GetSectionRelative(direction);
        if (targetTile != null && !targetTile.Occupied())
        {
            yield return StartCoroutine(movement.Move(targetTile, false));

            currentTile.SetCharacter(null);
            targetTile.SetCharacter(this);
            currentTile = targetTile;
        }
    }

    public void Die(){
        SC_BattleManager.Instance.RemoveCharacter(this);
        Destroy(gameObject);
    }
}


// implementations: TurnStart, Action, TurnEnd
public class CharacterTurnStart : IState
{
    protected SC_Character character;
    public CharacterTurnStart(SC_Character character)
    {
        this.character = character;
    }

    public virtual void Enter()
    {
    }

    public virtual IEnumerator Update()
    {
        Debug.Log($"{character.name} starts turn");
        yield return new WaitForSeconds(1.0f);
        yield return null;
    }

    public virtual void Exit()
    {
    }
}

public class CharacterAction : IState
{
    private SC_Character character;
    public CharacterAction(SC_Character character)
    {
        this.character = character;
    }

    public virtual void Enter()
    {
    }

    // major difference between player and enemy
    public virtual IEnumerator Update()
    {
        yield return null;
    }

    public virtual void Exit()
    {
    }
}

public class CharacterTurnEnd : IState
{
    private SC_Character character;
    public CharacterTurnEnd(SC_Character character)
    {
        this.character = character;
    }

    public virtual void Enter()
    {
    }

    public virtual IEnumerator Update()
    {
        Debug.Log($"{character.name} ends turn");
        yield return null;
    }

    public virtual void Exit()
    {
        SC_BattleManager.Instance.NextTurn();
    }
}