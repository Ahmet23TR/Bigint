using Fusion;
using UnityEngine;

public class RaycastAttackCaps : NetworkBehaviour
{
    public float Damage = 10;

    public CapsuleMovement CapsuleMovement;

    void Update()
    {
        if (HasStateAuthority == false)
        {
            return;
        }

        Ray ray = CapsuleMovement.Camera.ScreenPointToRay(Input.mousePosition);
        ray.origin += CapsuleMovement.Camera.transform.forward;

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.red, 1f);
            if (Runner.GetPhysicsScene().Raycast(ray.origin,ray.direction, out var hit))
            {
                if (hit.transform.TryGetComponent<Health>(out var health))
                {
                    health.DealDamageRpc(Damage);
                }
            }
        }
    }
}