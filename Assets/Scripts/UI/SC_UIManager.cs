using System.Collections.Generic;
using UnityEngine;

public class SC_UIManager : MonoBehaviour
{
    public GameObject CharacterIconPrefab;
    public Transform CharacterIconParent;

    private Dictionary<SC_Character, SC_CharacterIcon> characterIcons = new Dictionary<SC_Character, SC_CharacterIcon>();
    private SC_TurnOrderLayout turnOrderLayout;

    private void Start()
    {
        turnOrderLayout = FindObjectOfType<SC_TurnOrderLayout>();
    }

    public void InitializeQueue(LinkedList<SC_Character> characters)
    {
        foreach (SC_Character character in characters)
        {
            GameObject element = Instantiate(CharacterIconPrefab, CharacterIconParent);
            element.name = character.characterData.DisplayName;
            SC_CharacterIcon queueElement = element.GetComponent<SC_CharacterIcon>();
            queueElement.Initialize(character);
            characterIcons.Add(character, queueElement);
        }

        turnOrderLayout.UpdateLayout();

    }

    public void UpdateQueue(LinkedList<SC_Character> characters, LinkedListNode<SC_Character> currentTurnNode)
    {
        int index = 0;
        LinkedListNode<SC_Character> node = currentTurnNode;

        // Loop through the nodes starting from the currentTurnNode
        do
        {
            SC_CharacterIcon icon = characterIcons[node.Value];
            icon.transform.SetSiblingIndex(index++);
            node = node.Next ?? characters.First;
        }
        while (node != currentTurnNode);

        turnOrderLayout.UpdateLayout();
    }
}
