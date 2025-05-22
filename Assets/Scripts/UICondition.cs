using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Condition health;
    
    void Start()
    {
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }
}