using System.Collections.Generic;
using UnityEngine;

public class QuestsUi : MonoBehaviour
{
    [SerializeField] private GameObject _questItemUiPrefab;

    private List<QuestItemUi> _questItemUis = new List<QuestItemUi>();
    public void Initialize()
    {
        if (_questItemUis.Count >0 )
        {
            foreach (var questItemUi in _questItemUis)
            {
                Destroy(questItemUi.gameObject);
            }
        }

        _questItemUis = new List<QuestItemUi>();

        foreach (var quest in QuestsMechanic.Instance.GetQuests())
        {
            GameObject questItemGo = Instantiate(_questItemUiPrefab, transform);
            QuestItemUi questItemUi = questItemGo.GetComponent<QuestItemUi>();
            questItemUi.SetupNewQuest(quest,this);
            _questItemUis.Add(questItemUi);
        }
    }

    public void QuestCompleted(QuestItemUi questItemUi)
    {
        if (_questItemUis.Contains(questItemUi))
        {
            _questItemUis.Remove(questItemUi);
            Destroy(questItemUi.gameObject);
        }
    }
}
