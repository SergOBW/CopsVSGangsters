using UnityEngine;

public class StageNavigation : MonoBehaviour
{
    [SerializeField] private GameObject[] flags;
    public void SetPage(int index)
    {
        foreach (var gameObject in flags)
        {
            gameObject.SetActive(false);
        }
        
        flags[index].SetActive(true);
    }
}
