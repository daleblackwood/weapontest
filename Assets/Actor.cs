using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Actor : MonoBehaviour
{
    public enum Faction
    {
        Good = 1,
        Bad = 2
    }

    public Faction faction = Faction.Bad;
    public int health = 3;
    private float shake = 0.0f;
    private Vector3 knockVelocity = Vector3.zero;

    [SerializeField] private float hitCoolDownTime = 1.0f;

    private float lastHitTime = 0.0f; 

    public Vector3 headPosition => transform.position + Vector3.up;

    private void OnEnable()
    {
        ActorManager.Instance.Register(this);
    }
    
    protected virtual void Update()
    {
        if (knockVelocity.sqrMagnitude > 0.1f)
        {
            transform.position += knockVelocity;
            knockVelocity *= 0.9f;
        }
    }
    
    private void OnDisable()
    {
        ActorManager.Instance.Unregister(this);
    }

    public void TakeHit(Actor actor, int damage)
    {
        if (Time.time - lastHitTime < hitCoolDownTime)
            return;
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        lastHitTime = Time.time;
        knockVelocity = transform.position - actor.transform.position;
        knockVelocity.y = 0.0f;
        knockVelocity = knockVelocity.normalized * damage;
    }

    protected virtual void Die()
    {
        gameObject.SetActive(false);
    }
}
