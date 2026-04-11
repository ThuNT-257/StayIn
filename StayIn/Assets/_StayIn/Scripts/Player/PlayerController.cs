using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("Data")]
    [SerializeField]
    private PlayerStats playerStats;

    [Header("References")]
    [SerializeField]
    private InputReader InputReader;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveDirection;
    private Vector2 currentVelocity;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable() {
        if (InputReader != null) InputReader.MoveEvent += HandleMove;
    }

    private void Update() {
        UpdateAnimation();
    }

    private void FixedUpdate() {
        ApplyMovement();
    }

    private void OnDisable() {
        if (InputReader != null) InputReader.MoveEvent -= HandleMove;
    }

    private void HandleMove(Vector2 direction) {
        moveDirection = direction.normalized;

        if (moveDirection.x > 0) {
            transform.localScale = new Vector3(1, 1, transform.localScale.z);
        } else if (moveDirection.x < 0) {
            transform.localScale = new Vector3(-1, 1, transform.localScale.z);
        }
    }

    private void ApplyMovement() {
        Vector2 targetVelocity = moveDirection * playerStats.MoveSpeed;
        rb.linearVelocity = Vector2.SmoothDamp(
            rb.linearVelocity,
            targetVelocity,
            ref currentVelocity,
            playerStats.SmoothTime
        );
    }

    private void UpdateAnimation() {
        bool isMoving = moveDirection.sqrMagnitude > 0.01f;
        animator.SetBool("isMoving", isMoving);

        if (isMoving) {
            if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y)) {
                animator.SetFloat("AbsHorizontal", 1f);
                animator.SetFloat("Vertical", 0f);
            } else {
                animator.SetFloat("Vertical", moveDirection.y > 0 ? 1f : -1f);
                animator.SetFloat("AbsHorizontal", 0f);
            }
        } else {
            animator.SetFloat("AbsHorizontal", 0f);
            animator.SetFloat("Vertical", 0f);
        }
    }
}