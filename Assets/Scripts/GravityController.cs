using Fusion;
using UnityEngine;

public class GravityController : NetworkBehaviour
{
    private CharacterController _controller;
    private Vector3 _velocity;
    public float GravityValue = -9.81f;  // Sabit bir negatif yerçekimi değeri

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    public override void FixedUpdateNetwork()
    {
        ApplyGravity();  // Yerçekimini her zaman uygula
    }

    void ApplyGravity()
    {
        if (!_controller.isGrounded)
        {
            // Yerçekimi uygulama
            _velocity.y += GravityValue * Runner.DeltaTime;
            _controller.Move(new Vector3(0, _velocity.y, 0) * Runner.DeltaTime);
        }
        else
        {
            // Yere düştüğünde dikey hızı sıfırla
            _velocity.y = -1f;
        }
    }
}
