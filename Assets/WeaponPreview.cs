using UnityEngine;
using UnityEngine.Serialization;

public class WeaponPreview : MonoBehaviour
{
    [SerializeField]private GameObject[] gameObjects;
    public void ShowWeapon(int index)
    {
        foreach (var gameObject in gameObjects)
        {
            gameObject.SetActive(false);
        }
        
        gameObjects[index].SetActive(true);
    }
}
