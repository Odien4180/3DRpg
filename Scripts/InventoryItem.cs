using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;


public class InventoryItem : VisualElement
{
    public new class UxmlFactory : UxmlFactory<InventoryItem, UxmlTraits> { }
    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);

            var ate = ve as InventoryItem;

            ate.AddToClassList("background");

            var textArea = new VisualElement() { name = "background-text-area" };
            textArea.AddToClassList("background-text-area");
            ate.Add(textArea);

            var textMain = new Label() { name = "text-main", text = "Lv.1" };
            textMain.AddToClassList("text-main");
            textArea.Add(textMain);

            var iconArea = new VisualElement() { name = "background-icon-area" };
            iconArea.AddToClassList("background-icon-area");
            ate.Add(iconArea);

            var icon = new VisualElement() { name = "icon" };
            icon.AddToClassList("icon");
            iconArea.Add(icon);

            var starArea = new VisualElement() { name = "background-star-area" };
            starArea.AddToClassList("background-star-area");
            ate.Add(starArea);

            var star = new VisualElement() { name = "star" };
            star.AddToClassList("star");
            starArea.Add(star);

        }
    }

    public InventoryItem() : base()
    {
        
    }

    public async UniTask Initialize(ItemData itemData, UserItemData userItemData)
    {
        var starArea = this.Q<VisualElement>("background-star-area");

        for (int i = 0; i < itemData.grade - 1; ++i)
        {
            var star = new VisualElement() { name = "star" };
            star.AddToClassList("star");
            starArea.Add(star);
        }

        var icon = this.Q<VisualElement>("icon");
        var loadIcon = await Addressables.LoadAssetAsync<Sprite>(itemData.icon);
        
        icon.style.backgroundImage = new StyleBackground(loadIcon);
    }
}
