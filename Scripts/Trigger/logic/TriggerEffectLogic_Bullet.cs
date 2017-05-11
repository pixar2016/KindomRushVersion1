using System;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEffectLogic_Bullet : TriggerEffectBase
{
    public TriggerEffectLogic_Bullet()
    {

    }
    public override void ExcuteAction(TriggerInfo triggerInfo, TriggerEffectInfo effectInfo)
    {
        Debug.Log("TriggerEffectLogic_Bullet");
        CharacterInfo charInfo = triggerInfo.charInfo;
        CharacterInfo targetInfo = charInfo.GetAttackInfo();
        EntityManager.getInstance().AddBullet(1, charInfo, targetInfo, 200f, triggerInfo.triggerGroup.Id);
        //EntityManager.getInstance().AddMoveEffect(1, charInfo.GetPosition(), targetInfo.GetPosition(), 2.5f);
    }
}

