using UnityEngine;

namespace ForWeapon
{
    public class WeaponGraphicsMono : MonoBehaviour
    {
        private ParticleSystem muzzleFlash;
        private ParticleSystem hitImpact;
        
        private GameObject projectilePrefab;
        private GameObject weaponPrefab;
        public void Initialize(WeaponGraphics weaponGraphics)
        {
            if (weaponGraphics.projectilePrefab != null)
            {
                //projectilePrefab = Instantiate(weaponGraphics.projectilePrefab);
            }

            if (weaponGraphics.weaponPrefab != null)
            {
                weaponPrefab = Instantiate(weaponGraphics.weaponPrefab,transform);
            }

            if (weaponGraphics.muzzleFlash != null)
            {
                MuzzleFlashSlot muzzleFlashSlot = weaponPrefab.GetComponentInChildren<MuzzleFlashSlot>();
                if (muzzleFlashSlot != null)
                {
                    muzzleFlash = Instantiate(weaponGraphics.muzzleFlash,muzzleFlashSlot.transform);
                }
                else
                {
                    Debug.LogWarning("There is no muzzle flash slot");
                }
            }

            if (weaponGraphics.hitImpact != null)
            {
                //hitImpact = Instantiate(weaponGraphics.hitImpact);
            }

            transform.localPosition = weaponGraphics.Position;
            transform.localRotation = Quaternion.Euler(weaponGraphics.Rotation);
        }

        public void PlayMuzzle()
        {
            if (muzzleFlash == null)
            {
                return;
            }

            if (muzzleFlash.isPlaying)
            {
                muzzleFlash.Stop();
            }
            
            muzzleFlash.Play();
        }
        
        
    }
}