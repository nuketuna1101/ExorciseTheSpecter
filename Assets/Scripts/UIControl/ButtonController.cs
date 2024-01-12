using EasyTransition;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    /// <summary>
    /// ButtonController ::
    /// attached to each button UI
    /// </summary>
    private Button buttonUI;
    public enum ButtonType { LoadScene, PopUpWindow, CloseWindow, SelectCharacter, SelectChamber, EnterChamber, ExitGame }
    // 버튼 타입 정의
    [SerializeField]
    private ButtonType type;
    // 제어할 팝업창 게임오브젝트
    [SerializeField]
    private GameObject _PopUpWindow;
    // 씬 전환 관련코드
    private float startDelay = 0;
    [SerializeField]
    private TransitionSettings transition;
    [SerializeField]
    private string _sceneName;
    //
    [SerializeField]
    private int ChamberButtonNumbering;

    private void Awake()
    {
        // 버튼 타입별로 버튼 액션 기능 붙여주기
        buttonUI = this.transform.GetComponent<Button>();
        addButtonListner();
    }
    private void addButtonListner()
    {
        switch (type)   
        {
            case ButtonType.LoadScene:
                buttonUI.onClick.AddListener(LoadScene);
                break;
            case ButtonType.PopUpWindow:
                buttonUI.onClick.AddListener(PopUpWindow);
                break;
            case ButtonType.CloseWindow:
                buttonUI.onClick.AddListener(CloseWindow);
                break;
            case ButtonType.SelectChamber:
                buttonUI.onClick.AddListener(SelectChamber);
                break;
            case ButtonType.EnterChamber:
                buttonUI.onClick.AddListener(EnterChamber);
                break;
            case ButtonType.ExitGame:
                buttonUI.onClick.AddListener(ExitGame);
                break;
        }
    }

    private void LoadScene()
    {
        TransitionManager.Instance().Transition(_sceneName, transition, startDelay);

        AudioManager.Instance.PlaySFX(SFX_TYPE.BTN);
    }

    private void PopUpWindow()
    {
        // 팝업창 띄우기
        _PopUpWindow.SetActive(true);
    }
    private void CloseWindow()
    {
        // 닫기 버튼 눌러 팝업창 닫기
        _PopUpWindow.SetActive(false);
    }

    private void SelectChamber()
    {
        // 챔버 버튼을 눌렀을 때, 선택된 챔버 데이터 전달
        GameManager.Instance.CurSelectedChamberNumber = ChamberButtonNumbering;
        DebugOpt.Log("Btn Control :: CurSelectedChamberNumber => " + GameManager.Instance.CurSelectedChamberNumber);
    }

    private void EnterChamber()
    {
        // 선택된 챔버에 진입.
        //string _EnterScene = "3.ChamberView";
        string _EnterScene = "4.BattleScene";
        // 
        if (GameManager.Instance.CurSelectedChamberNumber == -1) return;

        GameManager.Instance.CurEnteredChamberNumber = GameManager.Instance.CurSelectedChamberNumber;
        GameManager.Instance.CurSelectedChamberNumber = -1;
        TransitionManager.Instance().Transition(_EnterScene, transition, 0);
        DebugOpt.Log("Btn Control :: CurSelectedChamberNumber => " + GameManager.Instance.CurSelectedChamberNumber);
        DebugOpt.Log("EnterChamber :: CurEnteredChamberNumber => " + GameManager.Instance.CurEnteredChamberNumber);

    }

    private void ConfirmCharacter()
    {
        //string _EnterScene = "2.MapView";
        //GameManager.Instance.CharacterCode;
        //TransitionManager.Instance().Transition(_EnterScene, transition, 0);
    }

    private void ExitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit(); // 어플리케이션 종료
    #endif
    }
}
