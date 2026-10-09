using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TankControl : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float turnSpeed = 140f;
    [SerializeField, Range(0f, 1f)] float reverseSpeedMultiplier = 0.75f;
    Rigidbody rb;
    float moveInput, turnInput;
    bool controlsEnabled = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints |= RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void Update()
    {
        if (!controlsEnabled || (GameManager.Instance != null && !GameManager.Instance.IsPlaying))
        { moveInput = turnInput = 0f; return; }
        moveInput = Input.GetAxisRaw("Vertical");   // W/S or Up/Down
        turnInput = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
    }

    void FixedUpdate()
    {
        if (!controlsEnabled || (GameManager.Instance != null && !GameManager.Instance.IsPlaying)) return;
        float speed = moveInput < 0 ? moveSpeed * reverseSpeedMultiplier : moveSpeed;
        rb.MovePosition(rb.position + transform.forward * moveInput * speed * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, turnInput * turnSpeed * Time.fixedDeltaTime, 0f));
    }

    public void SetControlsEnabled(bool enabled) { controlsEnabled = enabled; if (!enabled) moveInput = turnInput = 0f; }
}
