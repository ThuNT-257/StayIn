using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _smoothTime = 0.05f;

    [Header("References")]
    [SerializeField] private InputReader _inputReader;

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private Vector2 _moveDirection;
    private Vector2 _currentVelocity;

    private void Awake() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable() {
        if (_inputReader != null) _inputReader.MoveEvent += HandleMove;
    }

    private void Update() {
        UpdateAnimation();
    }

    private void FixedUpdate() {
        ApplyMovement();
    }

    private void OnDisable() {
        if (_inputReader != null) _inputReader.MoveEvent -= HandleMove;
    }

    private void HandleMove(Vector2 direction) {
        _moveDirection = direction.normalized;

        if (_moveDirection.x > 0) {
            transform.localScale = new Vector3(1, 1, transform.localScale.z);
        } else if (_moveDirection.x < 0) {
            transform.localScale = new Vector3(-1, 1, transform.localScale.z);
        }
    }

    private void ApplyMovement() {
        Vector2 targetVelocity = _moveDirection * _moveSpeed;
        _rigidbody2D.linearVelocity = Vector2.SmoothDamp(
            _rigidbody2D.linearVelocity,
            targetVelocity,
            ref _currentVelocity,
            _smoothTime
        );
    }

    private void UpdateAnimation() {
        if (_animator != null) {
            _animator.SetFloat("Speed", _moveDirection.magnitude);
        }
    }
}