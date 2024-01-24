# “ExorciseTheSpecter” (진행 중, 1/21일 기준)

# Unity 엔진 기반 모바일 Android 플랫폼 타겟

## 1) 프로젝트 개요

### **소개**: 1인 개발의 Unity 엔진 기반 싱글 플레이 덱빌딩 카드게임

### **프로젝트 제목**: Exorcise The Specter

## 2) 프로젝트 기획 및 기타

  시중에 존재하는 ‘하스스톤’, ‘궨트’, ‘마블스냅’, ‘슬레이 더 스파이어’ 등 고유로 정의한 룰 기반의 카드 게임을 만들어 보면 재밌겠다는 생각이 들어 진행해보게 되었습니다. 전반적인 게임의 진행 방식은 1인 개발인 만큼 싱글 플레이에 적합하도록 ‘슬레이 더 스파이어’ 게임을 모티브로 두었습니다.

개발을 하면서 필요한 주요 리소스들은 다음과 같은 방식으로 해결했습니다.

**배경 및 초상화 이미지** : Stable Diffusion을 통해 프롬프트로 생성한 생성 AI 이미지를 이용했습니다.

**배경음악과 효과음** : [OpenGameArt](https://opengameart.org/) 의 CC0 무료 라이센스에 해당하는 음성을 이용했습니다.

**UI 이미지 스프라이트** : OpenGameArt와 유니티 에셋 스토어의 무료 에셋을 이용했습니다.

  기본적으로 해당 게임은 다음과 같이 소개할 수 있습니다. 플레이어는 지정된 직업 클래스 중 하나를 선택하여 캐릭터를 선택하여 모험을 진행합니다. 플레이어는 여러 방으로 이루어진 스테이지를 끝까지 생존하여 클리어하는 것이 목표이며, 스테이지를 이루는 방은 적 유닛과의 전투, 덱에 추가할 카드를 구매하는 상점, 축복 혹은 저주에 대한 효과를 받을 수 있는 분수, 그리고 휴식을 취할 수 있는 모닥불 등으로 이루어져 있습니다. 방에 입장하여 전투를 진행할 때, 플레이어와 적 유닛은 턴을 주고 받으며 액션을 취하며, 플레이어는 자신의 덱을 통해 카드를 이용하여 적을 상대합니다. 이러한 로직을 메인으로 가지는 형식의 게임이라고 볼 수 있습니다.

## 3) 나는 무엇을 했는가?

프로젝트에서 현재까지 제가 진행한 내용은 다음과 같습니다.

### **게임 기획**

- 게임 룰, 게임 로직, 여러 가지 능력치 등에 대한 데이터 테이블 작성을 진행했습니다. 개발과 동시에 진행하는 만큼 수시로 기획서나 데이터 테이블 내용을 변경하였습니다.

### **프로젝트 개발**

다음은 제가 구현한 스크립트 일부를 소개한 내용입니다.

- Singleton 클래스 상속을 통해 여러 Manager 스크립트에 싱글턴 패턴 부여.
- CSVReader를 통한 csv 데이터 테이블로부터 데이터를 가져와 Scriptable Object에 담아 데이터 관리.
- ChamberManager : 그래프 자료구조로 스테이지의 챔버 노드 구현.
- UIManager를 통한 UI 관리 : StaticUI에 대한 바인딩과 가져오기와 비활성화되어 있는 PopUpUI에 대한 접근
- AudioManager를 통한 BGM, SFX 관리 : 다중 채널로 sfx 관리.
- Character : 전투 씬에서 상호작용을 주고 받은 플레이어와 전투 객체의 클래스와 메서드 정리.
- PoolManager를 통한 오브젝트 풀링 기법으로 카드 프리팹 관리 최적화
- DebugOpt를 통한 디버그 로그 메시지 최적화

## 4) 앞으로 무엇을 할 것인가?

다음은 앞으로 진행해야 할 목표와 추가적으로 업그레이드 가능한 방향성입니다.

- 전투 씬에 현재 적용된 로직에 대해 완전하게 게임 시스템 완성하기.
- 카드 관련 : 카드 사용효과에 대한 구현 마무리.
- 축복, 저주 관련한 속성과 메서드 추가 및 분수 방에서의 로직 구현.
- 상점 방과 휴식 방 구현.
- 완성 시 코드 리팩토링.

위의 항목들이 달성하고자하는 최소이며 다음과 같은 추가적인 방향성이 있습니다.

- 새로운 직업 클래스 및 카드 추가
- 새로운 카드 효과 키워드 추가
- 스테이지 챔버에 대한 랜덤 자동화 생성
- 랜덤 인카운터 방

이러한 추가 방향을 생각하며 코드를 재사용성 및 확장 가능하도록 고려하며 구현을 하고 있습니다.

## 5) 현재까지 구현된 데모 버전 시연



https://github.com/nuketuna1101/ExorciseTheSpecter/assets/103434648/f8319f89-5b4c-4e7b-b0b5-f6c5bda93d58
