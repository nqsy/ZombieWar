public enum EWeaponType
{
    weapon_1,
    weapon_2,
}

public static class WeaponTypeExtension
{
    public static ESoundType GetSoundType(this EWeaponType weaponType)
    {
        return weaponType switch
        {
            EWeaponType.weapon_1 => ESoundType.fire_weapon_1,
            EWeaponType.weapon_2 => ESoundType.fire_weapon_2,
        };
    }
}
