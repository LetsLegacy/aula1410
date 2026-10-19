using UnityEngine;
using Fusion;

public class ShootProjectile : NetworkBehaviour
{
    [SerializeField] float speed = 12f;
    [SerializeField] float lifeTime = 2.5f;
    [SerializeField] float hitRadius = 0.15f;

    float remainingLife;

    public override void Spawned()
    {
        remainingLife = lifeTime;
    }

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return;

        shootMatch match = ShootMatch.Instance;

        if (match != null && match.isOver)
        {
            Runner.Despawn(Object);
            return;
        }

        Vector3 movement = transform.forward * speed * Runner.DeltaTime;
        bool hitSomething = Physics.SphereCast(transform.position, hitRadius, movement.normalized, out RaycastHit hit,
            movement.magnitude, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);

        if (hitSomething)
        {
            ShootHealth target = hit.collider.GetComponentInParent<ShootHealth>();

            if(target != null) 
            {
                if(target.Object.StateAuthority == Object.StateAuthority) 
                {
                    transform.position += movement;
                    return;
                }
                target.RPC_ReceiveDamage();
                Runner.Despawn(Object);
                return;
            }

            Runner.Despawn(Object);
            return;

        }

        transform.position += movement;

    }

}
