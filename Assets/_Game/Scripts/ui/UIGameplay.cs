using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

public class UIGameplay : SingletonBehaviour<UIGameplay>
{
    [SerializeField] Joystick joystickController;

    [SerializeField] Button btnBomb;
    [SerializeField] Slider slHp;

    [SerializeField] TextMeshProUGUI txtTimer;
    [SerializeField] Button btnBack;

    [Header("Select weapon")]
    [SerializeField] Button btnSelectWeapon1;
    [SerializeField] GameObject selectWeapon1;

    [SerializeField] Button btnSelectWeapon2;
    [SerializeField] GameObject selectWeapon2;

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

        btnBack.OnClickAsObservable()
            .Subscribe(_ =>
            {
                TransitionEffect.instance.LoadMenuScene();
            }).AddTo(this);
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
}
