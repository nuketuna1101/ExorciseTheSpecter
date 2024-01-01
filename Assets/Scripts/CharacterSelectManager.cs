using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{
    // 캐릭터 : Rogue, Gunslinger
    private bool isSelectedAny = false;
    private int selectedCode = -1;
    private Button[] CharacterButtons;

    // 텍스트
    [SerializeField]
    private TMP_Text _Text;
    //
    private readonly Color[] colorArr = new Color[3];
    private readonly Color cc1 = new Color(1f, 1f, 0f);
    private readonly Color cc2 = new Color(175f/255f, 175f/255f, 0f);
    private readonly Color cc3 = new Color(125f/255f, 125f/255f, 0f);

    // 제어할 팝업 ui
    [SerializeField]
    private GameObject _PopUpInfoObj;
    private Image _CHAR_PORTRAIT_IMG;
    private TMP_Text _TEXT_CHAR_NAME;
    private TMP_Text _TEXT_CHAR_EXPLANATION;
    // 제어할 static ui 버튼
    [SerializeField]
    private GameObject _Start_Button;

    // 임시 :: 데이터 엑셀리딩 이전 레거시 버전
    // 
    [SerializeField]
    private Sprite[] TEMP_PopUpInfoObj;
    private string[] TEMP_TEXT_CHAR_NAME = { "ROGUE", "GUNSLINGER" };
    private string[] TEMP_TEXT_CHAR_EXPLANATION = { "ROGUE EXPLANATION", "GUNSLINGER EXPLANATION" };


    /*
     - 나중에 추가할 부분

    :: 팝업창 바뀔 때마다 진동 내지는 멀리서 팍 날아와서 꽂히는 이펙트
    :: 현재 선택되어진 직업창 버튼 폰트 반짝이기 + 폰트 그림자
     
     */


    private void Awake()
    {
        // 캐릭터 선택 버튼들 배열로 저장
        CharacterButtons = this.transform.gameObject.GetComponentsInChildren<Button>();
        // 색상코드 저장
        colorArr[0] = cc1;
        colorArr[1] = cc2;
        colorArr[2] = cc3;
        //
        StartCoroutine(FlashingText());
    }

    private IEnumerator FlashingText()
    {
        int i = 0;
        while (true)
        {
            // 오직 아무것도 선택 안되어 있을 때만.
            if (isSelectedAny) break;
            //
            yield return new WaitForSeconds(0.1f);
            _Text.color = colorArr[i];
            i = (i + 1) % colorArr.Length;
        }
    }

    public void ShowSelectedCharacter(int _characterCode)
    {
        // 최초 실행 시, 선택된 캐릭터 없다가 선택된 캐릭터가 생김
        if (!isSelectedAny)
        {
            isSelectedAny = true;
            // 캐릭터 정보 팝업창 띄우기
            _PopUpInfoObj.SetActive(true);
            //
            _CHAR_PORTRAIT_IMG = _PopUpInfoObj.transform.GetChild(0).GetChild(0).GetComponent<Image>();
            _TEXT_CHAR_NAME = _PopUpInfoObj.transform.GetChild(1).GetComponent<TMP_Text>();
            _TEXT_CHAR_EXPLANATION = _PopUpInfoObj.transform.GetChild(2).GetComponent<TMP_Text>();
            // 출정 버튼 활성화
            _Start_Button.SetActive(true);
        }

        // 이미 선택된 것이면 아무런 행동x
        if (selectedCode == _characterCode)
            return;

        // 변경 시 선택된 캐릭터 코드 수정
        selectedCode = _characterCode;

        // 내용 수정
        _CHAR_PORTRAIT_IMG.sprite = TEMP_PopUpInfoObj[selectedCode];
        _TEXT_CHAR_NAME.text = TEMP_TEXT_CHAR_NAME[selectedCode];
        _TEXT_CHAR_EXPLANATION.text = TEMP_TEXT_CHAR_EXPLANATION[selectedCode];
    }

    public void CompleteCharacterSelect()
    {
        // 캐릭터 선택 이후 시작버튼.

        // 테스트용 코드
        GameManager.Instance.characterCode = selectedCode;
        DebugOpt.Log("characterCode :: " + selectedCode);
    }

}
