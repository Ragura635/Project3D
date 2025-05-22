using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private float jumpPowerMultiflier;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Player"))
            return;

        // PlayerController 가져오기
        var player = CharacterManager.Instance.Player;
        if (player == null || player.controller == null) return;

        var controller = player.controller;
        var rb = controller.GetComponent<Rigidbody>();
        if (rb == null) return;

        // 수직 속도 초기화 후 트램폴린 튕김
        Vector3 velocity = rb.velocity;
        velocity.y = 0f;
        rb.velocity = velocity;

        rb.AddForce(Vector3.up * controller.jumpPower * jumpPowerMultiflier, ForceMode.Impulse);
    }
}
