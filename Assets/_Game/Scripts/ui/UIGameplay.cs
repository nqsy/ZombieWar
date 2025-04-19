using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

public class UIGameplay : SingletonBehaviour<UIGameplay>
{
    [SerializeField] Joystick joystickController;

    [SerializeField] Button btnBomb;
    [SerializeField] Button btnHeal;
    [SerializeField] Slider slHp;
    [SerializeField] TextMeshProUGUI txtHp;
 
    [SerializeField] TextMeshProUGUI txtTimer;
    [SerializeField] Button btnBack;

    [Header("Select weapon")]
    [SerializeField] Button btnSelectWeapon1;
    [SerializeField] GameObject selectWeapon1;

    [SerializeField] Button btnSelectWeapon2;
    [SerializeField] GameObject selectWeapon2;

    [Header("Victory")]
    [SerializeField] GameObject layoutVictory;
    [SerializeField] Button btnVictoryExit;

    [Header("Lose")]
    [SerializeField] GameObject layoutLose;
    [SerializeField] Button btnLoseExit;
    [SerializeField] Button btnRetry;

    private void Start()
    {
        TransitionEffect.instance.StartScene();

        //joystickController.
        SoldierObject.instance.weaponSelectRx
            .Subscribe(val =>
            {
                selectWeapon1.SetActive(val == EWeaponType.weapon_1);
                selectWeapon2.SetActive(val == EWeaponType.weapon_2);
            }).AddTo(this);

        SoldierObject.instance.hpRx
            .Subscribe(val =>
            {
                var progressHp = SoldierObject.instance.hpRx.Value / GameConfig.instance.maxHpSoldier;
                slHp.value = progressHp;

                txtHp.text = $"{SoldierObject.instance.hpRx.Value}/{GameConfig.instance.maxHpSoldier}";
            }).AddTo(this);

        btnSelectWeapon1.OnClickAsObservable()
            .Subscribe(_ =>
            {
                SoldierObject.instance.weaponSelectRx.Value = EWeaponType.weapon_1;
            }).AddTo(this);

        btnSelectWeapon2.OnClickAsObservable()
            .Subscribe(_ =>
            {
                SoldierObject.instance.weaponSelectRx.Value = EWeaponType.weapon_2;
            }).AddTo(this);

        btnBomb.OnClickAsObservable()
            .Subscribe(_ =>
            {
                SoldierObject.instance.SpawnBomb();
            }).AddTo(this);

        btnHeal.OnClickAsObservable()
            .Subscribe(_ =>
            {
                SoldierObject.instance.InscreaseHp(GameConfig.instance.healHp);
            }).AddTo(this);

        btnBack.OnClickAsObservable()
            .Subscribe(_ =>
            {
                TransitionEffect.instance.LoadMenuScene();
            }).AddTo(this);

        InitPopup();
    }

    private void Update()
    {
        SoldierObject.instance.UpdateMovement(joystickController.Vertical, joystickController.Horizontal);
        UpdateTimer();
    }

    void UpdateTimer()
    {
        var remain = GameplayManager.instance.cdPlayTime.Remain;

        var second = (int)remain % 60;
        var minute = (int)remain / 60;

        txtTimer.text = $"{minute}:{second}";
    }

    void InitPopup()
    {
        layoutVictory.SetActive(false);
        layoutLose.SetActive(false);

        btnVictoryExit.OnClickAsObservable()
            .Subscribe(_ =>
            {
                TransitionEffect.instance.LoadMenuScene();
            }).AddTo(this);

        btnLoseExit.OnClickAsObservable()
            .Subscribe(_ =>
            {
                TransitionEffect.instance.LoadMenuScene();
            }).AddTo(this);

        btnRetry.OnClickAsObservable()
            .Subscribe(_ =>
            {
                RetryUI();
            }).AddTo(this);
    }    

    void RetryUI()
    {
        layoutVictory.SetActive(false);
        layoutLose.SetActive(false);

        GameplayManager.instance.RetryGame();
    }    

    public void OpenVictoryPopup()

    {
        GameplayManager.instance.isPauseGame = true;
        layoutVictory.SetActive(true);
    }

    public void OpenLosePopup()
    {
        GameplayManager.instance.isPauseGame = true;
        layoutLose.SetActive(true);
    }
}
