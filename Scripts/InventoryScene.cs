using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryScene : MonoBehaviour
{
    private VisualElement root;
    public VisualTreeAsset itemTemplate;
    private ScrollView equipScrollview;
    public void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;


        equipScrollview = root.Q<ScrollView>("ScrollView");

        SetEquipScroll().Forget();

        //휠 할때마다 스크롤 오프셋이 컨테이너 사이즈에 따라 결정되는 부분 강제로 픽스
        equipScrollview.RegisterCallback<WheelEvent>((evt) =>
        {
            equipScrollview.scrollOffset = new Vector2(0, equipScrollview.scrollOffset.y + 500 * evt.delta.y);
            //이벤트 전파되지 않게 중지시킴
            evt.StopPropagation();
        });        
    }

    public async UniTask SetEquipScroll()
    {
        var itemList = InventoryManager.Instance.GetEquipInventory();

        for (int i = 0; i < itemList.Count; ++i)
        {
            var invenItem = itemTemplate.Instantiate();
            var itemData = await ItemInfo.Get(itemList[i].Value.itemId);
            await invenItem.Q<InventoryItem>().Initialize(itemData, itemList[i].Value);
            equipScrollview.contentContainer.Add(invenItem);
        }
    }

    private void InitializeItem(ItemData data)
    {
        
    }
}
