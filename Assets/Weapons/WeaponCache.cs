
using UnityEngine;

[CreateAssetMenu(menuName = "WeaponCache", fileName = "WeaponCache")]
public class WeaponCache : ScriptableObject
{
    [SerializeField] private Weapon[] weapons;

    public Weapon[] GetWeapons()
    {
        return weapons;
    }
}