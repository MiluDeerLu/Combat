using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateMachine
{
    public IState CurrentState { get; private set; }

    public IState TurnStart;
    public IState Action;
    public IState TurnEnd;

    private SC_Character character;

    public CharacterStateMachine(SC_Character character)
    {
        this.character = character;
    }

    public void StartTurn(IState startingState)
    {
        CurrentState = startingState;
        character.StartCoroutine(HandleStateSequence());
    }

    private IEnumerator HandleStateSequence()
    {
        while (CurrentState != null)
        {
            CurrentState.Enter();
            yield return character.StartCoroutine(CurrentState.Update());
            CurrentState.Exit();

            if (CurrentState == TurnStart)
            {
                CurrentState = Action;
            }
            else if (CurrentState == Action)
            {
                CurrentState = TurnEnd;
            }
            else if (CurrentState == TurnEnd)
            {
                CurrentState = null;
            }
        }
    }
}
