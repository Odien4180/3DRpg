# 3DRPG에 활용 가능한 기능 구현
3D RPG 장르에서 사용될 만한 클라이언트 기능들을 구현해보는 프로젝트입니다.
> [QuadMapUnit.cs](#quad-tree) : 쿼드트리를 이용한 오브젝트 관리<br>
> [InteractionBase.cs](#interaction) : 상호작용(아이템 루팅, NPC와의 대화) 구현<br>

사용한 외부 라이브러리


## Quad Tree
<a href="https://youtu.be/UrBnEAyCPYI">
	<p align="center"><img src="http://img.youtube.com/vi/UrBnEAyCPYI/0.jpg"></p>
  <p align="center">이미지 터치 시 유튜브 영상으로 이동합니다.</p>
<a><br>

유저와 상호작용 가능한 오브젝트들은 맵에 다수가 포함될수 있어, 동적으로 각각의 노드에 위치한 오브젝트의 숫자에 따라 동적으로 분할되는 쿼드트리를 통해 관리하였습니다.<br>
QuadMapUnit 컴포넌트가 부착된 오브젝는 QuadTree에 의해 관리되며, InteractionBase를 상속받은 컴포넌트에 따라 상호작용 동작이 정의 됩니다.<br>

[QuadTree.cs](https://github.com/Odien4180/3DRpg/blob/master/Scripts/QuadTree.cs)<br>
[QuadMapUnit.cs](https://github.com/Odien4180/3DRpg/blob/master/Scripts/QuadMapUnit.cs)<br>
[InteractionBase.cs](https://github.com/Odien4180/3DRpg/blob/master/Scripts/InteractionBase.cs)<br>

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

아이템 획득시 좌측 하단에 표시되는 아이템 정보 UI또한 InventoryManager에 작성된 Subject를 구독함으로써 아이템 획득 시 자동적으로 표시되도록 작업하였습니다.

[QuadMapUnit.cs](https://github.com/Odien4180/3DRpg/blob/master/Scripts/QuadMapUnit.cs)<br>
[InteractionView.cs](https://github.com/Odien4180/3DRpg/blob/master/Scripts/UI/InteractionView.cs)<br>
[InteractionPresenter.cs](https://github.com/Odien4180/3DRpg/blob/master/Scripts/UI/InteractionPresenter.cs)<br>
[InventoryManager.cs](https://github.com/Odien4180/3DRpg/blob/master/Scripts/Managers/InventoryManager.cs)<br>
