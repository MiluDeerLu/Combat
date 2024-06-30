using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SC_Movement
{
    private SC_Character character;

    // 我们想象一下移动速度在哪些场合会变化
    // 1. 根据角色定位本身基础移速就不一样 （比如重盾移速小于刺客，在初始化时候传）
    // 2. 特殊效果演出 (在call的时候做成optional parameter)
    // 3. 是否开了加速模式
    // 这样分析下来，角色传入的数据其实应该是1&2。而3应该从global config来fetch （是否加速&加的倍率）
    private float baseMoveSpeed;

    // movespeed is in Unity Unit per second
    public SC_Movement(SC_Character character, float baseMoveSpeed){
        this.character = character;
        this.baseMoveSpeed = baseMoveSpeed;
    }

    public IEnumerator Move(SC_Tile targetTile, bool ignoreOccupying, float moveSpeedMultiplier = 1){
        float moveSpeed = baseMoveSpeed * moveSpeedMultiplier * GlobalConfig.Instance.CurrentSpeedMultiplier;
    
        Vector3 targetPos = targetTile.transform.position;

        if(!ignoreOccupying && targetTile.Occupied()){
            Debug.Log("Target tile is occupied");
            yield break;
        }

        // move on xz plane
        Sequence moveSequence = DOTween.Sequence();
        moveSequence.Append(character.transform.DOMoveX(targetPos.x, 1/moveSpeed).SetEase(Ease.Linear));
        moveSequence.Join(character.transform.DOMoveZ(targetPos.z, 1/moveSpeed).SetEase(Ease.Linear));
        yield return moveSequence.WaitForCompletion();
    }

    public IEnumerator Move(Vector2Int direction, bool ignoreOccupying, float moveSpeedMultiplier){
        yield return null;
    }

   
}
