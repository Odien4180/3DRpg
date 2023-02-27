using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UniRx;
using UnityEngine;

public class QuadMapUnit : MonoBehaviour
{
    [Header("��ȣ�ۿ� ���� ����")]
    public InteractionBase interactionModule;

    [Space]

    private QuadNode node;
    public QuadNode Node 
    {
        get => node; 
        set => node = value;
    }
    public int debugDepth = -1;

    private Vector3 lastFramePos = Vector3.zero;
    //���� 4���� ��� �˻�
    private List<QuadNode> nNodes;
    public ReactiveProperty<QuadMapUnit> nearUnit = new ReactiveProperty<QuadMapUnit>();
    public FloatReactiveProperty nearUnitDistance;

    private void Start()
    {
        if (node == null)
        {
            node = WorldMapManager.Instance.FindNodes(transform.position,
                x => x.RegistUnit(GetInstanceID(), this));

            if (node == null)
            {
                return;
            }
        }
    }

    public void Update()
    {
        if (transform.position == lastFramePos)
        {
            return;
        }

        node.MoveCheck(this, GetInstanceID(), transform.position, ref node);

        GetAllNeighbor();

        lastFramePos = transform.position;
    }

    public void RemoveUnit()
    {
        node.RemoveUnit(GetInstanceID());
    }

    //���� ��忡 ��ġ�� ��ȣ�ۿ� ������ ���� ��Ī
    public void GetAllNeighbor()
    {
        nNodes = new List<QuadNode>();
        node.GetNeighbor(new XZ(1, 0), node.Depth, nNodes, null);
        node.GetNeighbor(new XZ(-1, 0), node.Depth, nNodes, null);
        node.GetNeighbor(new XZ(0, 1), node.Depth, nNodes, null);
        node.GetNeighbor(new XZ(0, -1), node.Depth, nNodes, null);
        nNodes = nNodes.Distinct().ToList();

        nearUnitDistance.Value = GetNearUnit(out var nearUnit);
        this.nearUnit.Value = nearUnit;
    }

    private float GetNearUnit(out QuadMapUnit nearUnit)
    {
        var nodeUnitData = node.GetNearUnit(this);
        nearUnit = nodeUnitData.Value;

        //���� ��忡 ���� ���� ��� �ֺ� ��� �˻� ���� ����
        if (nearUnit != null)
            return nodeUnitData.Key;

        float minDis = float.MaxValue;

        for (int i = 0; i < nNodes.Count; ++i)
        {
            var unitData = nNodes[i].GetNearUnit(this);

            if (unitData.Key < minDis)
            {
                minDis = unitData.Key;
                nearUnit = unitData.Value;
            }
        }

        return minDis;
    }

    public void Interaction()
    {
        nearUnit.Value?.interactionModule.Interaction(this);
    }

#if UNITY_EDITOR
    public bool debug = false;
    public float debugVisualHeight = 1.0f;
    private void OnDrawGizmos()
    {
        if (debug == false)
            return;

        if (node == null)
            return;

        float halfSize = node.Size / 2;

        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(new Vector3(node.LeftX + halfSize, transform.position.y + debugVisualHeight, 
            node.BottomZ + halfSize), new Vector3(node.Size, 0, node.Size));

        if (nNodes != null && nNodes.Count> 0)
        {
            foreach(var nNode in nNodes)
            {
                float nNodeHalfSize = nNode.Size / 2;
                Gizmos.color = new Color(0, 1, 0, 1.0f);
                Gizmos.DrawCube(new Vector3(nNode.LeftX + nNodeHalfSize, transform.position.y + debugVisualHeight,
                    nNode.BottomZ + nNodeHalfSize), new Vector3(nNode.Size, 0, nNode.Size));
            }
        }
    }
#endif
}
