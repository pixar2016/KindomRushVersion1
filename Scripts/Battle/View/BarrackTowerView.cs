﻿using System;
using System.Collections.Generic;
using Hero;
using UnityEngine;

public class BarrackTowerView : TowerView
{
    public Animate towerBase;
    public BarrackTowerView(BarrackTowerInfo towerInfo)
    {
        this.towerInfo = towerInfo;
        this.towerInfo.eventDispatcher.Register("DoAction", DoAction);
    }

    public override void LoadModel()
    {
        towerAsset = GameLoader.Instance.LoadAssetSync("Resources/Prefabs/SoliderTower.prefab");
        towerObj = towerAsset.GameObjectAsset;
        towerObj.transform.position = this.towerInfo.GetPosition();
        if (towerObj.GetComponent<ClickInfo>() == null)
        {
            ClickInfo clickInfo = towerObj.AddComponent<ClickInfo>();
            clickInfo.OnInit(ClickType.Tower, this.towerInfo.Id, FingerDown);
        }
        else
        {
            ClickInfo clickInfo = towerObj.GetComponent<ClickInfo>();
            clickInfo.OnInit(ClickType.Tower, this.towerInfo.Id, FingerDown);
        }
        if (towerObj.GetComponent<Animate>() != null)
        {
            towerBase = towerObj.GetComponent<Animate>();
        }
        else
        {
            towerBase = towerObj.AddComponent<Animate>();
        }
        towerBase.OnInit(towerInfo.towerBase);
        towerBase.startAnimation("idle");
    }

    public override void DoAction(object[] data)
    {
        string actionName = data[0].ToString();
        Debug.Log("BarrackTowerView  Do Action " + actionName);
        towerBase.startAnimation(actionName);
    }

    public override void Release()
    {
        GameLoader.Instance.UnLoadGameObject(towerAsset);
    }

    public override void Update()
    {
        this.towerObj.transform.position = this.towerInfo.GetPosition();
    }
}

