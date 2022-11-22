
using UnityEngine;

public class PickTargetWeapon : StraightLineWeapon
{
    private Actor target = null;
    
    protected override void OnFireBegin()
    {
        var target = ActorManager.Instance.GetTargetMostVisible(transform.position, owner.transform.forward, GetTargetFaction(), fireDistance);
        var targetPos = target.headPosition;
        SetProjectilePosition(targetPos);
        target.TakeHit(owner, damage);
    }
}