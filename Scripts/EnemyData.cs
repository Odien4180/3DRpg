using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Enemy Data", menuName = "ScriptableObject/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("�þ�")]
    public float sight = 15.0f;
    public float fov = 120.0f;
    [Header("�̵�")]
    public float speed = 1.0f;
    [Header("��Ÿ�")]
    public float range = 5.0f;
    public float rangeAngle = 30.0f;
    [Header("�ൿ ���� ���")]
    public float attackDelay = 1.0f;

    [Header("���� ����")]
    public int health = 100;
    public int power = 10;
    public int powerRange = 3;

    [Header("���� UI Offset")]
    public Vector3 healthGaguePos = new Vector3(0, 5, 0);
    public Vector3 healthGagueScale = new Vector3(1, 1, 1);

    [Header("��� ������")]
    public List<UserItemData> dropItems;
}
