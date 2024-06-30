using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleInitData", menuName = "ScriptableObjects/BattleInitData")]
public class SO_BattleInitData : ScriptableObject
{
    [Serializable]
    public struct CharacterInitData{
        public GameObject Prefab;
        public TilePosition Coordinate;
    }

    public List<CharacterInitData> InitData;
}
