using UnityEngine;

public class WeaponsPreview : MonoBehaviour
{
    [SerializeField] private GameObject[] weapons;

    private int currentWeaponIndex;

    private void Awake()
    {
        currentWeaponIndex = 0;
        SetWeapon(currentWeaponIndex);
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0,100f,0) * Time.deltaTime);
    }

    public void SetWeapon(int index)
    {
        if (index > weapons.Length)
        {
            return;
        }
        weapons[currentWeaponIndex].SetActive(false);
        currentWeaponIndex = index;
        weapons[currentWeaponIndex].SetActive(true);
    }
}
