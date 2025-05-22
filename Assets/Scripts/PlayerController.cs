using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Status")]
    public float fallDamageSpeedThreshold = -10f;
    public float fallDamageMultiplier = 2f;
    private bool wasGroundedLastFrame = true;
    
    [Header("Movement")]
    public float moveSpeed;
    public float jumpPower;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;
    private Rigidbody _rigidbody;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;
    public bool canLook = true;
    
    [Header("Item Effect")]
    public bool canUseItem = false;
    private bool isUsingItem = false;
    public ItemData currentItem;
    private Coroutine itemCoroutine;
    private float defaultJumpPower;
    private Vector3 defaultScale;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        defaultJumpPower = jumpPower;
        defaultScale = transform.localScale;
    }

    private void FixedUpdate()
    {
        Move();
        
        bool isGrounded = IsGrounded();

        // 착지 시점에만 실행
        if (!wasGroundedLastFrame && isGrounded)
        {
            float fallSpeed = _rigidbody.velocity.y;

            if (fallSpeed < fallDamageSpeedThreshold) // 예: -10보다 더 빠른 하강
            {
                float damage = Mathf.Abs(fallSpeed) * fallDamageMultiplier;
                ApplyFallDamage(damage);
            }
        }

        wasGroundedLastFrame = isGrounded;
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }
    
    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
    }
    
    private void ApplyFallDamage(float damage)
    {
        CharacterManager.Instance.Player.condition.TakeDamage(Mathf.RoundToInt(damage));
    }
    
    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            _rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)
        };

        for(int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }
    
    public void OnUseItem(InputAction.CallbackContext context)
    {
        if (!context.performed || !canUseItem || isUsingItem) return;

        if (currentItem != null)
        {
            itemCoroutine = StartCoroutine(ApplyItemEffect(currentItem));
        }
    }

    private IEnumerator ApplyItemEffect(ItemData item)
    {
        isUsingItem = true;
        canUseItem = false;
        UIManager.Instance.ShowUseButton(false);

        float duration = item.duration;
        float elapsedtime = 0f;
        
        transform.localScale = defaultScale * item.consumables[0].value;
        jumpPower = defaultJumpPower * item.consumables[1].value;

        UIManager.Instance.SetDurationFill(0f);

        while (elapsedtime < duration)
        {
            elapsedtime += Time.deltaTime;
            UIManager.Instance.SetDurationFill(elapsedtime / duration);
            yield return null;
        }

        // 원래대로 복원
        transform.localScale = defaultScale;
        jumpPower = defaultJumpPower;
        UIManager.Instance.SetDurationFill(1f);
        isUsingItem = false;
    }

    private void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}
