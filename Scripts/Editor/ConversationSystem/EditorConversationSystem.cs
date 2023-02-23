using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

public class EditorConversationSystem : EditorWindow
{
    VisualElement root;
    [MenuItem("Conversation/Conversation tool")]
    public static void ShowEditor()
    {
        EditorWindow editorWindow = GetWindow<EditorConversationSystem>();
        editorWindow.titleContent = new GUIContent("Conversation tool");
    }

    private void CreateGUI()
    {
        root = rootVisualElement;

        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/ConversationSystem/ConversationSystemTool.uxml");
        visualTree.CloneTree(root);

        Button loadBtn = root.Q<Button>("load-button");
        loadBtn.clickable.clicked += LoadConversationsForget;
    }

    private void LoadConversationsForget()
    {
        LoadConversations().Forget();
    }

    private async UniTask LoadConversations()
    {
        var locations = await Addressables.LoadResourceLocationsAsync("Conversation");
        var convAssets = await Addressables.LoadAssetsAsync<TextAsset>(locations, null);

        ScrollView scrollView = root.Q<ScrollView>("datas-scroll");
        scrollView.Clear();

        for (int i = 0; i < convAssets.Count; ++i)
        {
            Conversations.Clear();
            Conversations.Add(convAssets[i].name, convAssets[i]);
        }

        int index = 0;
        foreach(var conversation in Conversations.Dictionary)
        {
            Foldout fOut = new Foldout();
            fOut.text = conversation.Key;
            scrollView.Add(fOut);

            foreach(var convData in conversation.Value)
            {
                ConversationDataView convDataView = new ConversationDataView();
                convDataView.Initialize(convData.Value);
                fOut.Add(convDataView);

                if (index % 2 == 0)
                    convDataView.AddToClassList("convdata-view1");
                else
                    convDataView.AddToClassList("convdata-view2");

                ++index;
            }
        }
    }
}
