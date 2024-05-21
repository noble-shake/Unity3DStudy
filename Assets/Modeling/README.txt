해당 폴더는 용량이 큰
FBX 파일이 저장되는 공간이므로
깃허브 업로드 하지 않는다.

[해당 폴더안에 포함될 것]
Remy -  From MIXAMO
Remy@IdleWithSkin -  From MIXAMO
Remy@IdleWithoutSkin // 뼈대만 받는 버전 -  From MIXAMO
Animation Dance -  From MIXAMO
Animation Right Strafe
Animation Standard Run
ion Walking Backwareds

다운로드 받고 열어보면, 유니티에서는 찰흙 캐릭터로 보이게 됨
매트리얼이 들어가 있지 않거나, 연결되어있지 않은 상태기 때문
모델링 파일 안 매트리얼들은 잠겨 있음 (변형 불가능)
먼저 해제 해줘야 함
-Inspector -> Materials -> Extract Materials and Textures

Inspector -> Rig - Humarnoid 설정

애니메이션을 2개 동시에 동작하도록 한다면?
Animation Controller -> new From Blend Tree
blend type -> 2D Simple Directional
Input Manager 에서 값들의 민감도로 조정해도 되고..
그래비티를 직접 조정해도 되고. 자유롭게 가능

Foot IK -> 쳐다보거나, 시선을 고정한다던가 할 때, 애니메이션에 의해
조정하지 않게 바꿀 수 있음.
