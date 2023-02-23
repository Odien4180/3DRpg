using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ConversationDataArray
{
    public ConversationData[] datas;
}

[Serializable]
public struct ConversationData
{
    public int id;
    public string name;
    public string line;
}

public static class Conversations
{
    private static Dictionary<string, Dictionary<int, ConversationData>> conversationDic = 
        new Dictionary<string, Dictionary<int, ConversationData>>();
    public static Dictionary<string, Dictionary<int, ConversationData>> Dictionary => conversationDic;

    public static void Clear()
    {
        conversationDic.Clear();
    }

    public static void Add(string name, TextAsset textAsset)
    {
        if (conversationDic.ContainsKey(name) == false)
        {
            conversationDic.Add(name, new Dictionary<int, ConversationData>());
        }

        var datas = JsonUtility.FromJson<ConversationDataArray>(textAsset.ToString()).datas;

        for (int i = 0; i < datas.Length; ++i)
        {
#if UNITY_EDITOR
            Debug.Log($"Load Conversation\n{name} : {datas[i].id}\n{datas[i].name} : {datas[i].line}");
#endif
            conversationDic[name].Add(datas[i].id, datas[i]);
        }
    }

    public static ConversationData Get(string tag, int id)
    {
        return conversationDic[tag][id];
    }
}
