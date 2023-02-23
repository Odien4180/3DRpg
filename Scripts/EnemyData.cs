using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Enemy Data", menuName = "ScriptableObject/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("시야")]
    public float sight = 15.0f;
    public float fov = 120.0f;
    [Header("이동")]
    public float speed = 1.0f;
    [Header("사거리")]
    public float range = 5.0f;
    public float rangeAngle = 30.0f;
    [Header("행동 결정 요소")]
    public float attackDelay = 1.0f;

    [Header("스텟 정보")]
    public int health = 100;
    public int power = 10;
    public int powerRange = 3;

    [Header("연관 UI Offset")]
    public Vector3 healthGaguePos = new Vector3(0, 5, 0);
    public Vector3 healthGagueScale = new Vector3(1, 1, 1);

    [Header("드롭 아이템")]
    public List<UserItemData> dropItems;
}
