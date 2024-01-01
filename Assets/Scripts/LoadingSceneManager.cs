using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene;
    private int stateNo = 0;
    [SerializeField]
    Image loadingBar;
    [SerializeField]
    TMP_Text loadingText;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }
    public static void LoadScene(string sceneName)
    {
        // 정적 메소드로 어디서나 참조가능
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }
    IEnumerator LoadScene()
    {
        // 비동기로 씬을 이동시키면서, 로딩 화면 효과 시각화
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        float timer = 0.0f;

        while (!op.isDone)
        {
            yield return null;// 다음 프레임까지 대기
            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, op.progress * 0.5f, timer);
                changeText();
                if (loadingBar.fillAmount >= op.progress)
                    timer = 0f;
            }
            else
            {
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, 0.75f, timer);
                changeText();
                if (loadingBar.fillAmount == 0.75f)
                    break;
            }
        }
        while (loadingBar.fillAmount < 0.95f)
        {
            yield return null;// 다음 프레임까지 대기
            timer += Time.deltaTime;
            loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, 0.95f, timer);
            changeText();
        }
        op.allowSceneActivation = true;
        yield break;
    }
    private void changeText()
    {
        // 그저 효과를 위해서 텍스트 바꾸기
        switch (stateNo)
        {
            case 0:
                loadingText.text = "Loading..";
                stateNo = 1;
                break;
            case 1:
                loadingText.text = "Loading...";
                stateNo = 2;
                break;
            case 2:
                loadingText.text = "Loading....";
                stateNo = 0;
                break;
        }
    }
}
