using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameplay : SingletonBehaviour<UIGameplay>
{
    [SerializeField] Joystick joystickController;
    [SerializeField] Button btnSelectWeapon1;
    [SerializeField] Button btnSelectWeapon2;

    private void Start()
    {
        //joystickController.
    }

    private void Update()
    {
        SoldierObject.instance.UpdateMovement(joystickController.Vertical, joystickController.Horizontal);
    }
}
