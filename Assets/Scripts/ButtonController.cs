using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    // �� �̵��ϴ� ��ư�� �ٿ��ִ� ��ũ��Ʈ
    // ���� ���� ���� �̵��ϴ� �� �����ش�.
    private Button buttonUI;
    public enum ButtonType { PopUpWindow, CloseWindow, CharacterSelect }
    // ��ư Ÿ�� ����
    [SerializeField]
    private ButtonType type;
    // ������ �˾�â ���ӿ�����Ʈ
    [SerializeField]
    private GameObject _PopUpWindow;

    private void Awake()
    {
        buttonUI = this.transform.GetComponent<Button>();
        // ��ư Ÿ�Ժ��� ��ư �׼� ��� �ٿ��ֱ�
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
        // �˾�â ����
        _PopUpWindow.SetActive(true);
    }

    private void CloseWindow()
    {
        // �ݱ� ��ư ���� �˾�â �ݱ�
        _PopUpWindow.SetActive(false);
    }

}
