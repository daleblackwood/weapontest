using UnityEngine;

public class AOEWeapon : Weapon
{
    [SerializeField] private float range = 4.0f;

    private Actor[] hitActors;
    
    protected override void OnFireBegin()
    {
        var otherFaction = GetTargetFaction();
        projectile.transform.localScale = Vector3.one * range;
        projectile.transform.position = transform.position;
        hitActors = ActorManager.Instance.GetTargetsInRange(transform.position, otherFaction, range);
    }

    protected override void OnFireEnd()
    {
        base.OnFireEnd();
        foreach (var actor in hitActors)
        {
            actor.TakeHit(owner, damage);
        }
    }
}