using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/CharacterData")]
public class SO_CharacterData : ScriptableObject
{
    public string DisplayName;
    public Sprite DisplayImage;

    public uint MaxHP;
    public uint Speed;
}

public struct CharacterData{
    public string DisplayName;
    public Sprite DisplayImage;

    public uint MaxHP;
    public uint Speed;

    public bool Dead;

    public CharacterData(SO_CharacterData data){
        DisplayName = data.DisplayName;
        DisplayImage = data.DisplayImage;
        MaxHP = data.MaxHP;
        Speed = data.Speed;
        Dead = false;
    }
}