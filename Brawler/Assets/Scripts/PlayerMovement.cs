using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _forceValue = 6f;
    [SerializeField]
    private float _rotationSpeed = 500f;

    private float _horizontalInput;
    private float _verticalInput;
    [SerializeField]
    private float _groundDrag;

    private Vector3 _movementDirection;

    private Transform _orientation;
    private Vector3 _startPosition;

    private Rigidbody _rb;

    private float _playerHeight;
    [SerializeField]
    private LayerMask _whatIsGround;
    private bool _isGrounded;

    private Animator _animator;


    private void Awake()
    {
        Physics.gravity *= 1f;

        _orientation = transform.GetChild(2);

        _rb = GetComponent<Rigidbody>();

        _playerHeight = GetComponent<CapsuleCollider>().height;

        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        enabled = false;

        EventManager.Instance.OnStartGame += Enable;
        EventManager.Instance.OnStopGame += Disable;
        EventManager.Instance.OnStopGame += StopRunningAnimation;
        EventManager.Instance.OnRestartGame += Enable;
        EventManager.Instance.OnRestartGame += ResetToStartPosition;

        _startPosition = transform.position;
    }

    private void Update()
    {
        GroundCheck();
        PlayerInput();
        HandleDrag();
    }


    private void FixedUpdate()
    {
       if(_isGrounded) MoveAndRotatePlayer();
    }

    private void PlayerInput()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        if (Input.touchCount > 0)
        {
            _horizontalInput = Input.touches[0].deltaPosition.x;
            _verticalInput = Input.touches[0].deltaPosition.y;
        }
    }

    private void MoveAndRotatePlayer()
    {
        _movementDirection = Vector3.forward * _verticalInput + Vector3.right * _horizontalInput;
        _rb.AddForce(_movementDirection.normalized * _forceValue * 10f, ForceMode.Force);

        if (_movementDirection != Vector3.zero)
        {
            _animator.SetBool("isMoving", true);
            Quaternion toRotation = Quaternion.LookRotation(_movementDirection.normalized, _orientation.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _rotationSpeed);
        }
        else
        {
            _animator.SetBool("isMoving", false);
        }
    }

    private void GroundCheck()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, _whatIsGround);
    }

    private void HandleDrag()
    {
        if (_isGrounded)
        {
            _rb.drag = _groundDrag;
        }
        else
        {
            _rb.drag = 0f;
        }
    }

    private void Enable()
    {
        enabled = true;
    }

    private void Enable(EventManager.OnRestartGameEventArgs args)
    {
        enabled = true;
    }

    private void Disable()
    {
        enabled = false;
    }


    private void ResetToStartPosition(EventManager.OnRestartGameEventArgs args)
    {
        transform.position = _startPosition;
    }

    private void StopRunningAnimation()
    {
        _animator.SetBool("isMoving", false);
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnStartGame -= Enable;
        EventManager.Instance.OnStopGame -= Disable;
        EventManager.Instance.OnStopGame -= StopRunningAnimation;
        EventManager.Instance.OnRestartGame -= Enable;
        EventManager.Instance.OnRestartGame -= ResetToStartPosition;
    }
}
