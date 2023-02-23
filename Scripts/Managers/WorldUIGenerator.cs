using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(WorldUIGenerator))]
public class WorldUICanvasEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (Application.isPlaying == false) return;

        WorldUIGenerator generator = (WorldUIGenerator)target;

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        //if (GUILayout.Button("플로팅 텍스트 생성", GUILayout.Width(120), GUILayout.Height(30)))
        //{
        //    generator.PopFloatingText("test", new Vector3(0, 1, 0), Color.red);
        //}

        GUILayout.EndHorizontal();
    }
}
#endif

public class WorldUIGenerator : Singleton<WorldUIGenerator>
{
    public async UniTask PopFloatingText(string text, Vector3 position, Color baseColor, float randomPosRadius = 0.0f)
    {
        var floatingText = await ObjectPoolManager.Instance.Get<FloatingText>(Const.type_worldui, "FloatingText.prefab");
        floatingText.Initialize(text, position, baseColor, randomPosRadius);
    }

    public async UniTask PopDamageText(string text, Vector3 position, Color baseColor, float randomPosRadius = 0.0f)
    {
        var floatingText = await ObjectPoolManager.Instance.Get<FloatingText>(Const.type_worldui, "DamageText.prefab");
        floatingText.Initialize(text, position, baseColor, randomPosRadius);
    }
}
