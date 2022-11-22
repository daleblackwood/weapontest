using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public Actor owner { get; set; }

    public bool isAvailable => Time.time - fireStartTime > cooldownTime;
    
    [SerializeField] protected float cooldownTime = 2.0f;
    [SerializeField] protected float firingTime = 1.0f;
    [SerializeField] protected int damage = 1;
    [SerializeField] protected Transform projectilePrefab;
    
    protected Transform projectile;

    protected float fireStartTime = float.NegativeInfinity;
    protected float fireRemaining = 0.0f;

    private ParticleSystem particles;

    public void Fire()
    {
        if (owner == null)
        {
            Debug.LogWarningFormat("Weapon {0} needs an owner to fire.", name);
            return;
        }
        fireStartTime = Time.time;
        if (projectile == null && projectilePrefab != null)
        {
            projectile = GameObject.Instantiate(projectilePrefab);
        }
        if (projectile != null)
        {
            projectile.gameObject.SetActive(true);
            projectile.position = transform.position;
            projectile.forward = transform.forward;
        }
        fireRemaining = firingTime;
        try
        {
            OnFireBegin();
        }
        catch (Exception e)
        {
            if (projectile != null)
            {
                projectile.gameObject.SetActive(false);
            }
        }
    }

    private void Awake()
    {
        particles = GetComponentInChildren<ParticleSystem>();
        if (particles != null)
        {
            particles.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (fireRemaining > 0.0f)
        {
            fireRemaining -= Time.deltaTime;
            if (fireRemaining <= 0.0f)
            {
                OnFireEnd();
                fireRemaining = 0.0f;
            }
        }

        if (particles != null)
        {
            bool useParticles = isAvailable == false;
            if (useParticles != particles.gameObject.activeInHierarchy)
            {
                particles.gameObject.SetActive(useParticles);
            }
        }
    }
    
    protected abstract void OnFireBegin();

    protected virtual void OnFireEnd()
    {
        projectile.gameObject.SetActive(false);
    }

    protected Actor.Faction GetTargetFaction()
    {
        return owner.faction == Actor.Faction.Good ? Actor.Faction.Bad : Actor.Faction.Good;
    }

    private void OnEnable()
    {
        fireRemaining = 0.0f;
    }

    private void OnDisable()
    {
        projectile.gameObject.SetActive(false);
        if (particles != null)
        {
            particles.gameObject.SetActive(false);
        }
    }

    protected void SetProjectilePosition(Vector3 targetPos)
    {
        var diff = targetPos - transform.position;
        var distance = diff.magnitude;
        var scale = projectile.transform.localScale;
        scale.z = distance;
        projectile.localScale = scale;
        projectile.forward = diff.normalized;
        projectile.position = transform.position + diff * 0.5f;
    }
}
