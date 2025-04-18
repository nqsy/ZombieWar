using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UIMenu : SingletonBehaviour<UIMenu>
{

    [SerializeField] Button btnChooseMap1;
    [SerializeField] Button btnChooseMap2;

    private void Start()
    {
        btnChooseMap1.OnClickAsObservable()
            .Subscribe(_ =>
            {
                GameplayManager.mapId = 1;

                LoadGameplay();
            }).AddTo(this);

        btnChooseMap2.OnClickAsObservable()
            .Subscribe(_ =>
            {
                GameplayManager.mapId = 2;

                LoadGameplay();
            }).AddTo(this);
    }

    void LoadGameplay()
    {
        TransitionEffect.instance.LoadSceneMenuFromGameplay();
    }
}
