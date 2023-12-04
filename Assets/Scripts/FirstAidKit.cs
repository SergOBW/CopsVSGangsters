public class FirstAidKit : InteractableWithHealth
{
    protected override void Handle()
    {
        FindObjectOfType<PlayerCharacter>().DoHealthBonus();
        Destroy(gameObject);
    }
}
