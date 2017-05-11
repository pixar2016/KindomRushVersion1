using System;
using System.Collections.Generic;
using UnityEngine;

//子弹基类
public class BulletInfo
{
    //索引ID
    public int Id;
    //子弹ID
    public int bulletId;
    //子弹名称
    public string bulletName;
    //子弹速度
    public float speed;
    //子弹当前位置
    public Vector3 pos;
    //子弹当前角度
    public Quaternion quaternion;
    //发射子弹的塔或者兵种
    public CharacterInfo charInfo;
    public CharacterInfo targetInfo;
    //发射子弹的触发器Id
    public int triggerGroupId;

    public BulletInfo(int bulletIndexId, int _bulletId, CharacterInfo _charInfo, CharacterInfo _targetInfo, float _speed, int _triggerGroupId)
    {
        Id = bulletIndexId;
        bulletId = _bulletId;
        charInfo = _charInfo;
        targetInfo = _targetInfo;
        bulletName = "arrow";
        speed = _speed;
        pos = charInfo.GetPosition();
        triggerGroupId = _triggerGroupId;
    }

    public Vector3 GetPosition()
    {
        return pos;
    }

    public Quaternion GetEulerAngles()
    {
        return quaternion;
    }

    public void SetPosition(float x, float y, float z)
    {
        pos = new Vector3(x, y, z);
    }

    public void Update()
    {
        Vector3 targetPos = targetInfo.GetPosition();
        if (Vector3.Distance(pos, targetPos) < 1)
        {
            this.charInfo.eventDispatcher.Broadcast("BulletReach", triggerGroupId);
            EntityManager.getInstance().RemoveBullet(this.Id);
        }
        else
        {
            pos = Vector3.MoveTowards(pos, targetPos, speed * Time.deltaTime);
        }
    }
    public void Release()
    {

    }
}

