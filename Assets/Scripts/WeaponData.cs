using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon", order = 51)]
public class WeaponData : ScriptableObject
{
    [SerializeField]
    private string weaponName;

    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private float speed;

    public string WeaponName {
        get {
            return weaponName;
        }
    }

    public Sprite Icon {
        get {
            return icon;
        }
    }

    public float Speed {
        get {
            return speed;
        }
    }
}
