using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;

public class WorldMapManager : Singleton<WorldMapManager>
{
    private float cellSize;

    private QuadTree quadTree;

    private new void Awake()
    {
        base.Awake();
        InitializeGrid(-400.0f, -400.0f, 800.0f, 10, 6);
    }

    public void InitializeGrid(float posX, float poY, float nodeSize, int nodeMaxUnit, int maxDepth)
    {
        quadTree = new QuadTree(posX, poY, nodeSize, nodeMaxUnit, maxDepth);
    }

    private Vector3 WorldPosition(int x, int z)
    {
        return new Vector3(x, 0, z) * cellSize;
    }

    public QuadNode FindNodes(Vector3 pos, Action<QuadNode> onFind = null)
    {
        return quadTree.FindNodes(pos, onFind);
    }
}
