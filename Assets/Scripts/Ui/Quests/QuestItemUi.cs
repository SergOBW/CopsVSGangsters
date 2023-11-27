using Quests;
using Quests.HoldOnLevel;
using Quests.Hostage;
using Quests.Item;
using Quests.KillEnemy;
using Quests.LootMoney;
using TMPro;
using UnityEngine;

public class QuestItemUi : MonoBehaviour
{
    [SerializeField] private TMP_Text goalText;
    [SerializeField] private TMP_Text progressionText;

    private Quest _currentQuest;

    private QuestsUi _questsUi;

    public void SetupNewQuest(Quest quest, QuestsUi questsUi)
    {
        _currentQuest = quest;
        _questsUi = questsUi;
        
        _currentQuest.OnQuestCompleted += OnQuestCompleted;
        _currentQuest.OnQuestUpdated += UpdateUi;

        switch (LanguageManager.Instance.GetLanguage())
        {
            case Language.en:
                goalText.text = quest.enGoal;
                break;
            case Language.ru:
                goalText.text = quest.ruGoal;
                break;
            case Language.tr:
                goalText.text = quest.trGoal;
                break;
            default: goalText.text = quest.enGoal;
                break;
        }
        UpdateUi(quest);
    }

    private void OnQuestCompleted(Quest obj)
    {
        _currentQuest.OnQuestCompleted -= OnQuestCompleted;
        _currentQuest.OnQuestUpdated -= UpdateUi;
        _questsUi.QuestCompleted(this);
    }

    private void UpdateUi(Quest quest)
    {
        switch (quest)
        {
            case KillEnemyQuest killEnemyQuest:
                progressionText.text = killEnemyQuest.CurrentKillAmount + " / " + killEnemyQuest.KillAmount;
                break;
            case HostageQuest hostageQuest:
                progressionText.text = hostageQuest.CurrentHostageAmount + " / " + hostageQuest.HostagesAmount;
                break;
            case LootItemQuest lootItemQuest:
                progressionText.text = lootItemQuest.CurrentLootAmount + " / " + lootItemQuest.LootAmount;
                break;
            case LootMoneyQuest lootMoneyQuest :
                progressionText.text = $"{lootMoneyQuest.CurrentMoneyAmount} / {lootMoneyQuest.MoneyToLoot}";
                break;
            case HoldOnLevelQuest holdOnLevelQuest :
                progressionText.text = $"{(int)(holdOnLevelQuest.GetStartedTime() - holdOnLevelQuest.GetCurrentTime())}";
                break;
        }
    }
}
