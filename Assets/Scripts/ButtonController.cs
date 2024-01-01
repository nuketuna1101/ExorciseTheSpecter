using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    // 씬 이동하는 버튼에 붙여주는 스크립트
    // 현재 씬에 따라 이동하는 씬 정해준다.
    private Button buttonUI;
    public enum ButtonType { PopUpWindow, CloseWindow, CharacterSelect }
    // 버튼 타입 정의
    [SerializeField]
    private ButtonType type;
    // 제어할 팝업창 게임오브젝트
    [SerializeField]
    private GameObject _PopUpWindow;

    private void Awake()
    {
        buttonUI = this.transform.GetComponent<Button>();
        // 버튼 타입별로 버튼 액션 기능 붙여주기
        addButtonListner();
    }
    private void addButtonListner()
    {
        switch (type)   
        {

            case ButtonType.PopUpWindow:
                buttonUI.onClick.AddListener(PopUpWindow);
                break;
            case ButtonType.CloseWindow:
                buttonUI.onClick.AddListener(CloseWindow);
                break;

        }
    }
    private void SceneChanger()
    {
        /*
        string curSceneName = SceneManager.GetActiveScene().name;
        switch (curSceneName)
        {
            case "0.InitialScene":
                LoadingSceneManager.LoadScene("1.MainMenu");
                break;
            case "1.MainMenu":
                LoadingSceneManager.LoadScene("2.AreaView");
                break;
        }
        */
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

}
