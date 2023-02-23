using Cysharp.Threading.Tasks;
using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Scripting;
using UnityEngine.UIElements;
using static UnityEditor.FilePathAttribute;

public class ConversationDataView : VisualElement
{
    [Preserve]
    public new class UxmlFactory : UxmlFactory<ConversationDataView> { }

    private const string styleResource = "Assets/Scripts/Editor/ConversationSystem/ConversationSystemTool.uss";

    private ConversationData convData;

    private TextField idtf;
    private TextField nametf;
    private TextField linetf;

    public ConversationDataView()
    {
        styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(styleResource));
        AddToClassList("convdata-view");

        VisualElement textDataField = new VisualElement();
        textDataField.AddToClassList("text-field");
        hierarchy.Add(textDataField);

        //삭제 기능 추가
        VisualElement buttonField = new VisualElement();
        hierarchy.Add(buttonField);

        idtf = new TextField();
        idtf.label = "ID";
        idtf.ElementAt(0).AddToClassList("id-label");
        textDataField.Add(idtf);

        nametf = new TextField();
        nametf.label = "Name";
        nametf.ElementAt(0).AddToClassList("name-label");
        textDataField.Add(nametf);

        linetf = new TextField();
        linetf.label = "Line";
        linetf.ElementAt(0).AddToClassList("line-label");
        textDataField.Add(linetf);

        Button deleteButton = new Button();
        buttonField.Add(deleteButton);
    }

    public void Initialize(ConversationData convData)
    {
        this.convData = convData;

        idtf.value = convData.id.ToString();
        nametf.value = convData.name;
        linetf.value = convData.line;
    }
}
