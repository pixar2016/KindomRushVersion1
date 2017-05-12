using System;
using System.Collections.Generic;
using UnityEngine;
public class Bezier
{
    private Vector3 startPoint;
    private Vector3 endPoint;
    private Vector3 controlPoint;
    private int pathCount;
    private List<Vector3> pathList;
    public Bezier()
    {
        pathList = new List<Vector3>();
    }
    /// <summary>
    /// 计算一条Bezier路径
    /// </summary>
    /// <param name="startPoint">起点</param>
    /// <param name="endPoint">终点</param>
    /// <param name="count">路径点数</param>
    public void AddPath(Vector3 _startPoint, Vector3 _endPoint, int _count)
    {
        this.pathCount = _count;
        this.startPoint = _startPoint;
        this.endPoint = _endPoint;
        controlPoint = (_startPoint + _endPoint) / 2;
        controlPoint.y = controlPoint.y + Vector3.Distance(_startPoint, _endPoint);
        for (int i = 0; i < _count; i++)
        {
            pathList.Add(GetPointAtTime(i * 1.0f / _count));
        }
    }
    /// <summary>
    /// 得到贝塞尔曲线某个比例对应的点
    /// </summary>
    /// <param name="t">比例值（0-1）</param>
    /// <returns></returns>
    public Vector3 GetPointAtTime(float t)
    {
        Vector3 temp = (1 - t) * (1 - t) * startPoint + 2 * t * (1 - t) * controlPoint + t * t * endPoint;
        return temp;
    }
    /// <summary>
    /// 得到贝塞尔曲线某个点对应的坐标
    /// </summary>
    /// <param name="index">路径上某个点（0-路径总点数）</param>
    /// <returns></returns>
    public Vector3 GetPath(int index)
    {
        if (index >= 0 && index <= pathCount)
        {
            return pathList[index];
        }
        return Vector3.zero;
    }
    /// <summary>
    /// 重置
    /// </summary>
    public void Reset()
    {
        startPoint = Vector3.zero;
        endPoint = Vector3.zero;
        controlPoint = Vector3.zero;
        pathCount = 0;
        pathList.Clear();
    }
}

