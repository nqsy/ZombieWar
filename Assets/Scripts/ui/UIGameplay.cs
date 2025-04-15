using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UIGameplay : SingletonBehaviour<UIGameplay>
{
    [SerializeField] Joystick joystickController;

    [SerializeField] Button btnBomb;

    [Header("Select weapon")]
    [SerializeField] Button btnSelectWeapon1;
    [SerializeField] GameObject selectWeapon1;

    [SerializeField] Button btnSelectWeapon2;
    [SerializeField] GameObject selectWeapon2;

    private void Start()
    {
        //joystickController.
        SoldierObject.instance.weaponSelectRx
            .Subscribe(val =>
            {
                selectWeapon1.SetActive(val == 1);
                selectWeapon2.SetActive(val == 2);
            }).AddTo(this);

        btnSelectWeapon1.OnClickAsObservable()
            .Subscribe(_ =>
            {
                SoldierObject.instance.weaponSelectRx.Value = 1;
            }).AddTo(this);

        btnSelectWeapon2.OnClickAsObservable()
            .Subscribe(_ =>
            {
                SoldierObject.instance.weaponSelectRx.Value = 2;
            }).AddTo(this);

        btnBomb.OnClickAsObservable()
            .Subscribe(_ =>
            {
                SoldierObject.instance.SpawnBomb();
            }).AddTo(this);
    }

    private void Update()
    {
        SoldierObject.instance.UpdateMovement(joystickController.Vertical, joystickController.Horizontal);
    }
}
