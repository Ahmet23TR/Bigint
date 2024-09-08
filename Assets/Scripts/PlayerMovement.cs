using Fusion;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    private Vector3 _velocity;
    private bool _jumpPressed;
    private bool _jumpPressed2;
    private bool _attackPressed1;
    private bool _attackPressed2;
    private CharacterController _controller;
    private Animator _animator;
    public float WalkSpeed = 5f;
    public float RunSpeed = 10f;
    public float JumpForce = 5f;
    public float GravityValue = -8.5f;
    public Camera Camera;
    public float _rotationvelocity;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (HasStateAuthority == false)
        {
            return;
        }

        // Zıplama ve saldırı kontrolleri
        if (Input.GetButtonDown("Jump") && _controller.isGrounded)
        {
            _jumpPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.Z) && _controller.isGrounded)
        {
            _jumpPressed2 = true;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            _attackPressed1 = true;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            _attackPressed2 = true;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority == false)
        {
            return;
        }

        // Yerçekimini sürekli uygula
        if (!_controller.isGrounded)
        {
            _velocity.y += GravityValue * Runner.DeltaTime;  // Yerçekimi etkisi
        }

        // Zıplama işlemi
        if (_jumpPressed && _controller.isGrounded)
        {
            _velocity.y = JumpForce;  // Zıplama kuvvetini uygula
            _animator.SetTrigger("Jump");
        }

        if (_jumpPressed2 && _controller.isGrounded)
        {
            _velocity.y = JumpForce;  // Alternatif zıplama (ikinci buton)
            _animator.SetTrigger("Jump2");
        }

        // Hareketi uygula
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        move = Camera.transform.rotation * move;  // Kameranın yönüne göre hareket et
        move = move.normalized * ((_attackPressed1 || _attackPressed2) ? WalkSpeed : RunSpeed) * Runner.DeltaTime;

        _controller.Move(move + _velocity * Runner.DeltaTime);

        // Yere inildiğinde hızı sıfırla
        if (_controller.isGrounded)
        {
            _velocity.y = -1f;  // Hafif negatif bir değerle yere yapışmayı sağla
        }

        // Durumları sıfırlama
        _jumpPressed = false;
        _jumpPressed2 = false;
        _attackPressed1 = false;
        _attackPressed2 = false;
    }

    public override void Render()
    {
        if (HasStateAuthority == false)
        {
            return;
        }

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? RunSpeed : WalkSpeed;

        // Kamera yönünü almak ve hareket etmek
        Quaternion cameraRotationY = Quaternion.Euler(0, Camera.transform.eulerAngles.y, 0);
        Vector3 move = cameraRotationY * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * currentSpeed * Runner.DeltaTime;

        _controller.Move(move + _velocity * Runner.DeltaTime);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Animasyonları yönetme
        _animator.SetBool("isRunning", isRunning);
        _animator.SetBool("isWalking", new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).magnitude > 0 && !isRunning);

        // Koşarken saldırı yapılamaz
        if (_attackPressed1 && !isRunning)
        {
            _animator.SetTrigger("Attack");
        }
        if (_attackPressed2 && !isRunning)
        {
            _animator.SetTrigger("Attack2");
        }
    }


    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            Camera = Camera.main;
            Camera.GetComponent<ThirdPersonCamera>().Target = transform;
        }
    }
}