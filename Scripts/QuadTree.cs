using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.AddressableAssets.Build;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using UnityEngine.InputSystem.HID;
using UnityEngine.InputSystem;
using System.Linq;
using UniRx;

public struct XZ
{
    public int x;
    public int z;
    public XZ(int x, int z)
    {
        this.x = x; 
        this.z = z;
    }

    public static XZ operator +(XZ a, XZ b)
    {
        return new XZ(a.x + b.x, a.z + b.z);
    }
    public static XZ operator -(XZ a, XZ b)
    {
        return new XZ(a.x - b.x, a.z - b.z);
    }
    public static XZ operator -(XZ a)
    {
        return new XZ(-a.x, -a.z);
    }

    public XZ Reverse()
    {
        if (x == 0) x = 1;
        else x = 0;
        if (z == 0) z = 1;
        else z = 0;

        return this;
    }
}

public class QuadNode : IDisposable
{
    public int maxUnit = 1;
    public XZ index;

    private int depth;
    public int Depth => depth;
    private int maxDepth;
    private float nodeSize;
    
    public float Size => nodeSize;

    private QuadNode[,] childs;

    //private Dictionary<int, QuadMapUnit> unitDictionary
    //    = new Dictionary<int, QuadMapUnit>();

    public Dictionary<int, QuadMapUnit> unitDictionary
        = new Dictionary<int, QuadMapUnit>();

    private QuadNode parent;

    // left x
    private float lx;
    public float LeftX => lx;
    // right x
    private float rx;
    public float RightX => rx;
    // bottom z
    private float bz;
    public float BottomZ => bz;
    // top z
    private float tz;
    public float TopZ => tz;

    private bool leaf = false;
    

    public QuadNode(QuadNode parent, float x, float z, float nodeSize, int maxUnit, int maxDepth, XZ index, int currentDepth = 0)
    {
        this.depth = currentDepth;
        this.maxDepth = maxDepth;
        this.nodeSize = nodeSize;

        this.parent = parent;

        lx = x;
        rx = x + nodeSize;
        bz = z;
        tz = z + nodeSize;

        this.maxUnit = maxUnit;
        this.index = index;

        leaf = true;

        if (parent != null)
        {
            foreach(var unit in parent.unitDictionary)
            {
                Vector3 unitPos = unit.Value.transform.position;

                if (InCheck(unitPos.x, unitPos.z))
                {
                    RegistUnit(unit.Key, unit.Value);
                    unit.Value.Node = this;
                }
            }
        }

        Debug.DrawLine(new Vector3(lx, 0, bz), new Vector3(rx, 0, bz), Color.white, 100f);
        Debug.DrawLine(new Vector3(lx, 0, bz), new Vector3(lx, 0, tz), Color.white, 100f);
    }

    private void InitializeChild()
    {
        leaf = false;

        float childNodeSize = nodeSize / 2;
        childs = new QuadNode[2, 2];
        childs[0, 0] = new QuadNode(this, lx, bz,
            childNodeSize, maxUnit, maxDepth, new XZ(0, 0), depth + 1);
        childs[1, 0] = new QuadNode(this, lx + childNodeSize, bz,
            childNodeSize, maxUnit, maxDepth, new XZ(1, 0), depth + 1);
        childs[0, 1] = new QuadNode(this, lx, bz + childNodeSize,
            childNodeSize, maxUnit, maxDepth, new XZ(0, 1), depth + 1);
        childs[1, 1] = new QuadNode(this, lx + childNodeSize, bz + childNodeSize,
            childNodeSize, maxUnit, maxDepth, new XZ(1, 1), depth + 1);

        unitDictionary.Clear();
    }

    public QuadNode FindNode(Vector3 pos, Action<QuadNode> onFind = null)
    {
        if (InCheck(pos.x, pos.z) == false)
        {
            return null;
        }

        if (leaf)
        {
            onFind?.Invoke(this);
            return this;
        }

        for (int x = 0; x < childs.GetLength(0); ++x)
        {
            for (int z = 0; z < childs.GetLength(1); ++z)
            {
                var node = childs[x, z].FindNode(pos, onFind);

                if (node != null)
                    return node;
            }
        }

        return null;
    }

    public void MoveCheck(QuadMapUnit unit, int instanceId, Vector3 pos, ref QuadNode node)
    {
        if (InCheck(pos.x, pos.z))
        {
            node = FindNode(pos, x => {
                x.RegistUnit(instanceId, unit);
                });

            return;
        }

        if (leaf)
        {
            RemoveUnit(instanceId);
        }
        

        if (parent != null)
            parent.MoveCheck(unit, instanceId, pos, ref node);
#if UNITY_EDITOR
        else
            Debug.LogError("Unit place out of QuadTree");
#endif
    }

    public bool InCheck(float x, float z)
    {
        return lx < x && x <= rx &&
        bz < z && z <= tz;
    }

    public void RegistUnit(int instanceId, QuadMapUnit unit)
    {
        if (unitDictionary.ContainsKey(instanceId))
        {
            return;
        }

        unitDictionary.Add(instanceId, unit);

        if (unitDictionary.Count > maxUnit && depth < maxDepth)
        {
            foreach(var kv in unitDictionary)
            {
                if (kv.Value == null ||
                    kv.Value.IsDestroyed())
                    unitDictionary.Remove(kv.Key);
            }

            if (unitDictionary.Count > maxUnit)
            {
                InitializeChild();
                unitDictionary.Clear();
            }
        }
    }

    public void RemoveUnit(int instanceId)
    {
        if (unitDictionary.ContainsKey(instanceId))
            unitDictionary.Remove(instanceId);
    }

    public KeyValuePair<float, QuadMapUnit> GetNearUnit(QuadMapUnit centerUnit)
    {
        KeyValuePair<float, QuadMapUnit> minDisData = new KeyValuePair<float, QuadMapUnit>(float.MaxValue, null);
        
        foreach (var unit in unitDictionary)
        {
            if (centerUnit.GetInstanceID() == unit.Key)
                continue;

            float dis = Vector3.Distance(unit.Value.transform.position, centerUnit.transform.position);
            
            if (dis <= unit.Value.interactionModule.interactionDistance && dis < minDisData.Key)
            {
                minDisData = new KeyValuePair<float, QuadMapUnit>(dis, unit.Value);
            }
        }

        return minDisData;
    }

    public QuadNode GetNode(int depth)
    {
        if (this.depth == depth)
            return this;
        else
            return parent.GetNode(depth);
    }

    public void GetNeighbor(XZ index, int depth, List<QuadNode> nodes, Stack<XZ> path) 
    {
        if (path == null)
            path = new Stack<XZ>();

        if (parent == null)
            return;

        var targetIndex = this.index + index;

        if (targetIndex.x < 0 ||
            targetIndex.z < 0 ||
            targetIndex.x > 1 ||
            targetIndex.z > 1)
        {
            path.Push(this.index);

            parent.GetNeighbor(index, depth, nodes, path);
            return;
        }        

        parent.childs[targetIndex.x, targetIndex.z].GetNeighborChilds(-index, depth, nodes, path);

        return;
    }

    private void GetNeighborChilds(XZ index, int depth, List<QuadNode> nodes, Stack<XZ> path)
    {
        if (leaf)
        {
            nodes.Add(this);
            return;
        }

        XZ reverseLastIndex = new XZ(0, 0);
        if (path.Count > 0)
            reverseLastIndex = path.Pop();

        // ªÛ«œ
        if (index.x == 0)
        {
            int z = index.z;
            if (z == -1)
                z = 0;

            if (this.depth < depth)
            {
                childs[reverseLastIndex.x, z].GetNeighborChilds(index, depth, nodes, path);
            }
            else
            {
                childs[0, z].GetNeighborChilds(index, depth, nodes, path);
                childs[1, z].GetNeighborChilds(index, depth, nodes, path);
            }
        }
        // ¡¬øÏ
        else if (index.z == 0)
        {
            int x = index.x;
            if (x == -1)
                x = 0;
            if (this.depth < depth)
            {
                childs[x, reverseLastIndex.z].GetNeighborChilds(index, depth, nodes, path);
            }
            else
            {
                childs[x, 0].GetNeighborChilds(index, depth, nodes, path);
                childs[x, 1].GetNeighborChilds(index, depth, nodes, path);
            }
        }
    }
    
    //private void GetDiagonalNeighbor(XZ index, XZ childIndex, int depth, List<QuadNode> nodes)
    //{
    //    if (parent == null)
    //        return;
    //
    //    var targetIndex = this.index + index;
    //
    //    if (targetIndex.x < 0 ||
    //        targetIndex.z < 0 ||
    //        targetIndex.x > 1 ||
    //        targetIndex.z > 1)
    //    {
    //        parent.GetDiagonalNeighbor(index, childIndex, depth, nodes);
    //        return;
    //    }
    //
    //    parent.childs[targetIndex.x, targetIndex.z].GetDiagonalNeighborChilds(childIndex, nodes);
    //
    //    return;
    //}
    //
    //private void GetDiagonalNeighborChilds(XZ childIndex, List<QuadNode> nodes)
    //{
    //    if (leaf)
    //    {
    //        nodes.Add(this);
    //        return;
    //    }
    //
    //    childs[childIndex.x, childIndex.z].GetDiagonalNeighborChilds(childIndex, nodes);
    //}

    public void Dispose()
    {
        for (int i = 0; i < childs.GetLength(0); ++i)
        {
            for (int j = 0; j < childs.GetLength(1); ++j)
            {
                childs[i, j].Dispose();
            }
        }

        unitDictionary.Clear();
        unitDictionary = null;

        parent = null;
    }
}

public class QuadTree
{
    private QuadNode root;

    public QuadTree(float x, float y, float nodeSize, int maxUnit, int maxDepth)
    {
        root = new QuadNode(null, x, y, nodeSize, maxUnit, maxDepth, new XZ(0, 0));
    }

    public QuadNode FindNodes(Vector3 pos, Action<QuadNode> onFind = null)
    {
        return root.FindNode(pos, onFind); 
    }
}
