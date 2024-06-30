using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SC_CharacterIcon : MonoBehaviour
{
    public TMP_Text CharacterNameContainer;
    public Image CharacterImageContainer;

    public SC_Character Character { get; private set;}

    public void Initialize(SC_Character character){
        Character = character;
        UpdateUI();
    }

    public void UpdateCharacter(SC_Character character){
        Character = character;
        UpdateUI();
    }

    private void UpdateUI(){
        CharacterNameContainer.text = Character.characterData.DisplayName;
        CharacterImageContainer.sprite = Character.characterData.DisplayImage;
    }
}
