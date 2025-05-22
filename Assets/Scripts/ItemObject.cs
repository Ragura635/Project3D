using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;
    private bool isHeld = false;
    private Transform playerCam;
    private Rigidbody rb;
    private float holdDistance = 2f;
    private float moveSpeed = 10f;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        if (isHeld && playerCam != null)
        {
            Vector3 targetPos = playerCam.position + playerCam.forward * holdDistance;
            Vector3 smoothed = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * moveSpeed);
            rb.MovePosition(smoothed);
        }
    }
    
    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}";
        return str;
    }
    
    public string GetInteractAction()
    {
        string str = isHeld ? "Drop" : "Pick Up";
        return str;
    }
    
    public void OnInteract()
    {
        if (!isHeld)
        {
            PickUp();
        }
        else
        {
            Drop();
        }
    }
    
    private void PickUp()
    {
        isHeld = true;
        playerCam = Camera.main.transform;
        holdDistance = Vector3.Distance(playerCam.position, transform.position);
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.drag = 10f;
    }

    private void Drop()
    {
        isHeld = false;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
        rb.drag = 0f;
        playerCam = null;
    }
}
