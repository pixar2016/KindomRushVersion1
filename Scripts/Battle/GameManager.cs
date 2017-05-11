using System;
using System.Collections.Generic;
using UnityEngine;
using CoreLib.Timer;

public class GameManager
{
    private static GameManager instance = null;
    //当前回合
    public int curWave;
    public int wave;
    public int hp;
    public int gold;
    //存储所有路径信息
    public PathLoader pathLoader;
    //存储怪物出场顺序
    public MonsterSequence monsterSeq;
    //当前波所有怪物
    Dictionary<int, List<int>> curWaveMonsters;
    //key-pathID  value-当前轮到第几个怪物
    public Dictionary<int, int> pathMonster;

    private CTimerSystem m_TimerSystem = null;
    //当前正在点击的物体
    public ClickInfo curClickInfo;
    private GameManager()
    {
        m_TimerSystem = new CTimerSystem();
        m_TimerSystem.Create();
        curWave = 0;
        pathLoader = new PathLoader();
        monsterSeq = new MonsterSequence();
        pathMonster = new Dictionary<int, int>();
    }

    public static GameManager getInstance()
    {
        if (instance == null)
        {
            instance = new GameManager();
        }
        return instance;
    }

    public void LoadLevel(int levelNum)
    {
        D_Map mapData = J_Map.GetData(levelNum);
        wave = mapData._wave;
        hp = mapData._hp;
        gold = mapData._gold;
        //根据路径名字加载数据
        pathLoader.LoadPath(mapData._pathJson);
        //加载怪物数据
        monsterSeq.LoadData(mapData._monsterJson);
        //根据数据加载塔空位
        AddTowerByData();
    }
    public void AddTowerByData()
    {
        PathInfo towerPosInfo = pathLoader.GetPath("Tower");
        int count = towerPosInfo.GetCount();
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = towerPosInfo.GetPoint(i);
            TowerInfo tower = EntityManager.getInstance().AddTower(100);
            tower.SetPosition(pos.x, pos.y, 0);
            tower.ChangeState("idle");
        }
    }
    //开始游戏
    public void StartGame()
    {
        Debug.Log("StartGame");
        StartNextWave(100);
        //每隔5s开始一波敌人
        m_TimerSystem.CreateTimer(100, 5000, StartNextWave);
    }

    //开始出下一波怪物
    public void StartNextWave(uint nTimeID)
    {
        Debug.Log(curWave);
        Debug.Log(wave);
        if (curWave >= wave)
        {
            Debug.Log("wave reach max");
            m_TimerSystem.DestroyTimer(100, StartNextWave);
            return;
        }
        curWave++;
        curWaveMonsters = monsterSeq.GetMonsterByWave(curWave);
        foreach (int pathId in curWaveMonsters.Keys)
        {
            //重置计数
            if (!pathMonster.ContainsKey(pathId))
            {
                pathMonster.Add(pathId, 0);
            }
            else
            {
                pathMonster[pathId] = 0;
            }
            //每条路径创建一个定时器，定时产生怪物
            m_TimerSystem.CreateTimer((uint)pathId, 1000, AddMonsters);
        }
    }

    public void AddMonsters(uint nTimeID)
    {
        Debug.Log("AddMonsters" + nTimeID);
        int pathId = (int)nTimeID;
        //pathId路径轮到第几个怪物
        int monsterNum = pathMonster[pathId];
        //pathId路径上怪物列表
        List<int> monsters = curWaveMonsters[pathId];
        if (monsterNum >= monsters.Count)
        {
            Debug.Log("DestroyTimer " + nTimeID);
            m_TimerSystem.DestroyTimer((uint)pathId, AddMonsters);
            return;
        }
        MonsterInfo monster = EntityManager.getInstance().AddMonster(monsters[monsterNum], pathLoader.GetPath(pathId.ToString()));
        monster.ChangeState("move");
        pathMonster[pathId]++;
    }

    public void Update()
    {
        if (null != m_TimerSystem)
        {
            m_TimerSystem.UpdateTimer();
        }
    }

}

