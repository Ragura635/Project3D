using UnityEngine;

public class ItemGiveButton : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = CharacterManager.Instance.Player.controller;
            player.canUseItem = true;
            UIManager.Instance.ShowUseButton(true);
            UIManager.Instance.SetDurationFill(0f);
        }
    }
}
