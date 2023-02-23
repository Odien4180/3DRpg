using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Temp Inventory Data", menuName = "ScriptableObject/Temp Inventory Data")]
public class TempInventoryData : ScriptableObject
{
    public List<UserItemData> dataList;
}
