using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SC_TurnOrderLayout : MonoBehaviour
{
    private List<SC_CharacterIcon> characterIcons = new List<SC_CharacterIcon>();

    public float SpaceBetweenIcons;
    public float MoveDuration = 0.5f; 

    [ContextMenu("Update Layout")]
    public void UpdateLayout()
    {
        Debug.Log("Updating layout");

        characterIcons = new List<SC_CharacterIcon>(GetComponentsInChildren<SC_CharacterIcon>());

        // each object tween to new position
        Vector3 currentPos = transform.position;
        foreach (SC_CharacterIcon icon in characterIcons)
        {
            icon.transform.DOMove(currentPos, MoveDuration);
            currentPos.y -= SpaceBetweenIcons;
        }
    }
}
