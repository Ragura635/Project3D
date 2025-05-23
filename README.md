## 🎮 주요 기능

### 🧍‍♂️ 1. 기본 이동 및 점프
- `PlayerController`로 WASD 이동, 마우스 회전, 스페이스바 점프 가능
- `Rigidbody` 기반 물리 적용
- `IsGrounded()`로 정밀한 바닥 체크

---

### 🚀 2. 트램폴린 발판
- `Trampoline.cs`를 통해 플레이어와 충돌 시 트램폴린 효과 적용
- 수직 속도를 0으로 초기화한 뒤, `jumpPower * multiplier` 만큼 위로 튕겨냄
- 이를 통해 기존 점프보다 더 높은 곳으로 이동 가능

```csharp
velocity.y = 0f;
rb.velocity = velocity;
rb.AddForce(Vector3.up * controller.jumpPower * jumpPowerMultiflier, ForceMode.Impulse);
```

---

### 🧪 3. 아이템 들기 및 놓기
- `ItemObject`가 `IInteractable` 인터페이스로 작동
- `Interaction.cs`를 통해 플레이어가 카메라 중심으로 줍고, 들고 다니며, 드롭 가능

```csharp
public void OnInteract()
{
    if (!isHeld) PickUp(); else Drop();
}
```

---

### 💥 4. 아이템 효과 시스템
- `PlayerController` 내 `ApplyItemEffect()` Coroutine 사용
- 사용 시 일정 시간 동안:
  - **크기 증가**
  - **점프력 증가**
- UI Fill Amount로 지속 시간 시각화

```csharp
transform.localScale = defaultScale * item.consumables[0].value;
jumpPower = defaultJumpPower * item.consumables[1].value;
```
