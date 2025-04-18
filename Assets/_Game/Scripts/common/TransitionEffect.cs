using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionEffect : SingletonBehaviour<TransitionEffect>
{
    [SerializeField] Image bg;
    [SerializeField] float durationEffect = 2;
    [SerializeField] float durationEffectPopup = 2;

    bool isLoadScreen = false;

    Color nonBlack = Color.black;

    private void Start()
    {
        var color = nonBlack;
        color.a = 0f;
        nonBlack = color;
    }

    void CompleteRunEffect()
    {
        bg.gameObject.SetActive(false);
    }

    public void LoadMenuScene()
    {
        LoadScene("2-menu");
    }

    public void LoadGameplayScene()
    {
        LoadScene("3-gameplay");
    }

    public void LoadScene(string nameScene, UnityAction onCallBack = null)
    {
        isLoadScreen = true;
        bg.gameObject.SetActive(true);
        bg.color = nonBlack;
        bg.DOColor(Color.black, durationEffect)
            .OnComplete(() =>
            {
                onCallBack?.Invoke();
                SceneManager.LoadScene(nameScene, LoadSceneMode.Single);
            });
    }

    public void StartScene()
    {
        isLoadScreen = false;
        bg.color = Color.black;
        bg.gameObject.SetActive(true);
        bg.DOColor(nonBlack, durationEffect)
            .OnComplete(() =>
            {
                CompleteRunEffect();
            });
    }
}
