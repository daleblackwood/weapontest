using UnityEngine;

public class StraightLineWeapon : Weapon
{
    [SerializeField] protected float fireDistance = 10.0f;
    [SerializeField] protected float pointRadius = 1.0f;
    
    protected override void OnFireBegin()
    {
        var targetPos = transform.position + transform.forward * fireDistance;
        SetProjectilePosition(targetPos);
        var target = ActorManager.Instance.GetTargetClosest(targetPos, GetTargetFaction(), pointRadius);
        if (target != null)
        {
            target.TakeHit(owner, damage);
        }
    }
}