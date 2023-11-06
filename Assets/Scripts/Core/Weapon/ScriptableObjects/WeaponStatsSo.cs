using ForWeapon;
using UnityEngine;

public abstract class WeaponStatsSo : ScriptableObject
{
    public float damage;
    public float range;
    public float fireRate;
    public AnimatorOverrideController animatorOverrideController;
    public float DistanceOffset = 0.1f;
    
    public AudioClip[] audioClips;

    public abstract Weapon Initialize();
}