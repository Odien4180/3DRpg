# 3DRPG에 활용 가능한 기능 구현
3D RPG 장르에서 사용될 만한 클라이언트 기능들을 구현해보는 프로젝트입니다.
지속적으로 업데이트 될 예정입니다<br>
>Last update : 23/03/01
<br>
목차
<br>

1. [Object Pooling](#object-pooling) : 사용성을 위해 Addressable asset system과 결합시킨 오브젝트 풀링 기능 구현<br>
2. [Quad Tree](#quad-tree) : 쿼드트리를 이용한 오브젝트 관리<br>
3. [Interaction](#interaction) : Rx기반 상호작용(아이템 루팅, NPC와의 대화) 구현<br><br>

사용한 외부 라이브러리<br>
+ [UniRx](https://github.com/neuecc/UniRx)
+ [UniTesk](https://github.com/Cysharp/UniTask)

## Object Pooling
자주 사용하게 될 기능이니 최대한 사용하기 편하게 만드는 것에 중점을 두고 작성한 ObjectPooling 기능 입니다. 
동적으로 오브젝트를 생성시키는 것과 관련되있는 만큼 사용 시 각 오브젝트 별 오브젝트 풀 최초 생성 시 Addressable asset system을 통해 에셋을 로드 및 풀 생성을 진행하도록 작업했습니다.
```c#
//사용 예시) Object pooling을 통한 데미지 텍스트 출력 시 사용되는 코드 입니다.
await ObjectPoolManager.Instance.Get<FloatingText>(Const.type_worldui, "DamageText.prefab");
//이처럼 한줄로 최대한 간편하게 사용할 수 있도록 작업하였습니다.
```

>연관 클래스<br>
1. [ObjectPoolManager.cs](https://github.com/Odien4180/3DRpg/blob/master/Scripts/Managers/ObjectPoolManager.cs)<br>
2. [AddressableManager.cs](https://github.com/Odien4180/3DRpg/blob/master/Scripts/Managers/AddressableManager.cs)<br>

## Quad Tree
<a href="https://youtu.be/UrBnEAyCPYI">
	<p align="center"><img src="http://img.youtube.com/vi/UrBnEAyCPYI/0.jpg"></p>
  <p align="center">이미지 터치 시 유튜브 영상으로 이동합니다.</p>
<a><br>

유저와 상호작용 가능한 오브젝트들은 맵에 다수가 포함될 수 있어, 각각의 노드에 위치한 오브젝트의 숫자에 따라 동적으로 분할되는 쿼드트리를 통한 공간 파티션 방식으로 관리하였습니다.<br>
QuadMapUnit 컴포넌트가 부착된 오브젝는 QuadTree에 의해 관리되며, InteractionBase를 상속받은 컴포넌트에 따라 상호작용 동작이 정의 됩니다.<br>
	
	
>연관 클래스<br>
1. [QuadTree.cs](https://github.com/Odien4180/3DRpg/blob/master/Scripts/QuadTree.cs)<br>
2. [QuadMapUnit.cs](https://github.com/Odien4180/3DRpg/blob/master/Scripts/QuadMapUnit.cs)<br>
3. [InteractionBase.cs](https://github.com/Odien4180/3DRpg/blob/master/Scripts/InteractionBase.cs)<br>

## Interaction
<a href="https://youtu.be/NWD6PxnmOfU">
	<p align="center"><img src="http://img.youtube.com/vi/NWD6PxnmOfU/0.jpg"></p>
  <p align="center">이미지 터치 시 유튜브 영상으로 이동합니다.</p>
<a><br>

상호작용 동작에 관련 된 코드들로 아이템 루팅, NPC와의 대화를 구현했습니다.<br>
캐릭터가 이동할 때마다 QuadMapUnit의 nearUnit값이 수정되고, MVP패턴에 따라 해당 값을 구독하고 있는 InteractionPresenter가 InteractionView를 통해 인접해 있는 상호작용 가능한 오브젝트 정보를 표시해 주게 됩니다.<br>

```c#
public class QuadMapUnit : MonoBehaviour
{
    ...
    
    public ReactiveProperty<QuadMapUnit> nearUnit = new ReactiveProperty<QuadMapUnit>();
}
```
```c#
public class InteractionPresenter : MonoBehaviour
{
    public InteractionView view;
    private IDisposable currentRx;
    
    ...
    
    private void SetRx()
    {
        currentRx?.Dispose();
        currentRx = unit.nearUnit.AsObservable().Subscribe(x =>
        {
            view.Remove();
            if (x != null)
            {
                var nearUnit = unit.nearUnit.Value;
                view.Pop(nearUnit.interactionModule.interactionName);
            }
        }).AddTo(this);
    }
}
```

아이템 획득시 좌측 하단에 표시되는 아이템 정보 UI또한 InventoryManager에 작성된 Subject를 구독함으로써 아이템 획득 시 자동적으로 표시되도록 작업하였습니다.<br>
	
	
>연관 클래스<br>
1. [QuadMapUnit.cs](https://github.com/Odien4180/3DRpg/blob/master/Scripts/QuadMapUnit.cs)<br>
2. [InteractionView.cs](https://github.com/Odien4180/3DRpg/blob/master/Scripts/UI/InteractionView.cs)<br>
3. [InteractionPresenter.cs](https://github.com/Odien4180/3DRpg/blob/master/Scripts/UI/InteractionPresenter.cs)<br>
4. [InventoryManager.cs](https://github.com/Odien4180/3DRpg/blob/master/Scripts/Managers/InventoryManager.cs)<br>
