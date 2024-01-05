using EasyTransition;
using System;
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
    public enum ButtonType { LoadScene, PopUpWindow, CloseWindow, SelectCharacter, SelectChamber, EnterChamber }
    // ��ư Ÿ�� ����
    [SerializeField]
    private ButtonType type;
    // ������ �˾�â ���ӿ�����Ʈ
    [SerializeField]
    private GameObject _PopUpWindow;


    // �� ��ȯ �����ڵ�
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
        // ��ư Ÿ�Ժ��� ��ư �׼� ��� �ٿ��ֱ�
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
        }
    }

    private void LoadScene()
    {
        TransitionManager.Instance().Transition(_sceneName, transition, startDelay);
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
        // è�� ��ư�� ������ ��, ���õ� è�� ������ ����
        GameManager.Instance.CurSelectedChamberNumber = ChamberButtonNumbering;
        DebugOpt.Log("Btn Control :: cur selected charNo => " + GameManager.Instance.CurSelectedChamberNumber);
    }

    private void EnterChamber()
    {
        //

    }
}
