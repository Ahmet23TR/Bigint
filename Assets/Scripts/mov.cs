using Fusion;
using UnityEngine;

public class mov : NetworkBehaviour
{
    private Vector3 _velocity;
    private bool _jumpPressed;
    private bool _attackPressed1;
    private bool _attackPressed2;
    private CharacterController _controller;
    private Animator _animator;
    private bool isGrounded = false; // Yere temas kontrolü için
    public float WalkSpeed = 5f;
    public float RunSpeed = 10f;
    public float JumpForce = 5f;
    public float GravityValue = -9.81f;
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
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            _jumpPressed = true;
            _animator.SetTrigger("Jump");
        }
        if (Input.GetKeyDown(KeyCode.Z) && isGrounded)
        {
            _jumpPressed = true;
            _animator.SetTrigger("Jump2");
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

        if (isGrounded)
        {
            _velocity = new Vector3(0, -1, 0);
        }

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? RunSpeed : WalkSpeed;

        // Y ekseni etrafında kamera yönünü almak
        Quaternion cameraRotationY = Quaternion.Euler(0, Camera.transform.eulerAngles.y, 0);
        

        Vector3 move = cameraRotationY * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Runner.DeltaTime * currentSpeed;

        _velocity.y += GravityValue * Runner.DeltaTime;

        if (_jumpPressed && isGrounded)
        {
            _velocity.y += JumpForce;
        }

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

        // Durumları sıfırlama
        _jumpPressed = false;
        _attackPressed1 = false;
        _attackPressed2 = false;
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            Debug.Log("Temas var");
            
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            Debug.Log("Temas yok");
            
            isGrounded = false;
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
