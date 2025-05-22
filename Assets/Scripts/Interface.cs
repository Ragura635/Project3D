public interface IInteractable
{
    public string GetInteractPrompt();
    public string GetInteractAction();
    public void OnInteract();
}

public interface IDamagable
{
    void TakeDamage(int damage);
}