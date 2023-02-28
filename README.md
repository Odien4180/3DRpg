# 3DRPG에 활용 가능한 기능 구현
3D RPG 장르에서 사용될 만한 클라이언트 기능들을 구현해보는 프로젝트입니다.

## Quad Tree
<a href="https://youtu.be/UrBnEAyCPYI">
	<p align="center"><img src="http://img.youtube.com/vi/UrBnEAyCPYI/0.jpg"></p>
  <p align="center">이미지 터치 시 유튜브 영상으로 이동합니다.</p>
<a><br>
유저와 상호작용 가능한 오브젝트들은 맵에 다수가 포함될수 있어, 동적으로 각각의 노드에 위치한 오브젝트의 숫자에 따라 동적으로 분할되는 쿼드트리를 통해 관리하였습니다.<br>
QuadMapUnit 컴포넌트가 부착된 오브젝는 QuadTree에 의해 관리되며, ItemInteraction을 상속받은 컴포넌트에 따라 상호작용 동작이 정의 됩니다.<br>

[QuadTree.cs](https://github.com/Odien4180/3DRpg/blob/master/Scripts/QuadTree.cs)<br>
[QuadMapUnit.cs](https://github.com/Odien4180/3DRpg/blob/master/Scripts/QuadMapUnit.cs)<br>
[ItemInteraction.cs](https://github.com/Odien4180/3DRpg/blob/master/Scripts/ItemInteraction.cs)<br>

 
