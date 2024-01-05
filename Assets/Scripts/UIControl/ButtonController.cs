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
    public enum ButtonType { PopUpWindow, CloseWindow, SelectCharacter, SelectChamber, EnterChamber }
    // ��ư Ÿ�� ����
    [SerializeField]
    private ButtonType type;
    // ������ �˾�â ���ӿ�����Ʈ
    [SerializeField]
    private GameObject _PopUpWindow;

    private void Awake()
    {
        // ��ư Ÿ�Ժ��� ��ư �׼� ��� �ٿ��ֱ�
        buttonUI = this.transform.GetComponent<Button>();
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
            case ButtonType.SelectChamber:
                buttonUI.onClick.AddListener(SelectChamber);
                break;
            case ButtonType.EnterChamber:
                buttonUI.onClick.AddListener(EnterChamber);
                break;
        }
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

    private void SelectChamber()
    {
        //

    }
    private void EnterChamber()
    {
        //

    }
}
