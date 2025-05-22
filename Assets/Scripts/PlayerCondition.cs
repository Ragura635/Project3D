using System;
using UnityEngine;

public class PlayerCondition : MonoBehaviour, IDamagable
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }

    public float noHungerHealthDecay;
    public event Action onTakeDamage;

    private void Update()
    {
        health.Add(health.passiveValue * Time.deltaTime);

        if(health.curValue < 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("죽음 ㅠㅠ");
    }
    
    public void TakeDamage(int damage)
    {
        health.Substract(damage);
        onTakeDamage?.Invoke();
    }
}