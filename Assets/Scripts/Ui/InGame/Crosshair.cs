using EnemyCore;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField]private RectTransform rectTransform;
    [SerializeField] private GameObject hitMarker;
    [SerializeField] private GameObject headHitMarker;
    private PlayerCharacter characterBehaviour;
    
    public float size;

    private void OnEnable()
    {
        hitMarker.SetActive(false);
        headHitMarker.SetActive(false);
        size = 64f;
            
        //Get Player Character.
        characterBehaviour = FindObjectOfType<PlayerCharacter>();
    }

    private void Awake()
    {
        if (EnemyHandleMechanic.Instance != null)
        {
            EnemyHandleMechanic.Instance.OnEnemyHit += OnEnemyHit;
        }
    }

    private void OnEnemyHit(bool isHead)
    {
        if (isHead)
        {
            HitHead();
        }
        else
        {
            Hit();
        }
    }

    private void Start()
    {
        hitMarker.SetActive(false);
        headHitMarker.SetActive(false);
    }
    
    public void Shot()
    {
        size += 25f;
    }
    void Update()
    {
        if (characterBehaviour != null)
        {
            if (!characterBehaviour.IsAiming())
            {
                rectTransform.gameObject.SetActive(true);
            }
            else
            {
                rectTransform.gameObject.SetActive(false);
            }
        }
        
        rectTransform.sizeDelta = new Vector2(size, size);
        
        if (size <= 64f)
        {
            size = 64f;
            return;
        }

        size -= 350f * Time.deltaTime;

    }

    public void Hit()
    {
        hitMarker.SetActive(true);
        headHitMarker.SetActive(false);
        Invoke("Clear",0.3f);
    }

    public void HitHead()
    {
        hitMarker.SetActive(false);
        headHitMarker.SetActive(true);
        Invoke("Clear",0.3f);
    }

    private void Clear()
    {
        hitMarker.SetActive(false);
        headHitMarker.SetActive(false);
    }
}
