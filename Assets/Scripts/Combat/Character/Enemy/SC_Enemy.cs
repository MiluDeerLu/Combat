using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Tasks.Actions;
using UnityEngine;

public class SC_Enemy : SC_Character
{
    protected override void InitializeStates()
    {
        turnState.TurnStart = new EnemyTurnStart(this);
        turnState.Action = new EnemyAction(this);
        turnState.TurnEnd = new EnemyTurnEnd(this);
    }
}

public class EnemyTurnStart : CharacterTurnStart
{
    private SC_Enemy enemy;
    public EnemyTurnStart(SC_Enemy character) : base(character)
    {
        enemy = character;
    }
}

public class EnemyAction : CharacterAction
{
    private SC_Enemy enemy;
    public EnemyAction(SC_Enemy character) : base(character)
    {
        enemy = character;
    }

    public override IEnumerator Update()
    {
        yield return base.Update();

        enemy.Move(Vector2Int.down);

        Debug.Log($"{enemy.name} is taking action");
    }
}

public class EnemyTurnEnd : CharacterTurnEnd
{
    private SC_Enemy enemy;
    public EnemyTurnEnd(SC_Enemy character) : base(character)
    {
        enemy = character;
    }
}
