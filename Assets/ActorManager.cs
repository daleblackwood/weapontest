using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class ActorManager : MonoBehaviour
{
    private const float DEFAULT_SEARCH_DISTANCE = 30.0f;
    
    private static ActorManager instance;

    public static ActorManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("ActorManager").AddComponent<ActorManager>();
            }
            return instance;
        }
    }

    [SerializeField] private List<Actor> actors = new List<Actor>();

    private void Awake()
    {
        instance = this;
    }
    
    public void Register(Actor actor)
    {
        if (actors.Contains(actor))
            return;
        
        actors.Add(actor);
    }

    public void Unregister(Actor actor)
    {
        if (actors.Contains(actor) == false)
            return;
        
        actors.Remove(actor);
    }

    public Actor[] GetTargetsInRange(Vector3 position, Actor.Faction faction, float range = DEFAULT_SEARCH_DISTANCE)
    {
        var results = new List<Actor>();
        foreach (var actor in actors)
        {
            if (actor.faction != faction)
                continue;
            var diff = actor.transform.position - position;
            if (diff.sqrMagnitude < range * range)
            {
                results.Add(actor);
            }
        }
        return results.ToArray();
    }

    public Actor GetTargetClosest(Vector3 position, Actor.Faction faction, float maxDistance = DEFAULT_SEARCH_DISTANCE)
    {
        Actor result = null;
        float closestDistSq = maxDistance * maxDistance;
        foreach (var actor in actors)
        {
            if (actor.faction != faction)
                continue;
            var diff = actor.transform.position - position;
            var distSq = diff.sqrMagnitude;
            if (distSq < closestDistSq)
            {
                result = actor;
                closestDistSq = distSq;
            }
        }
        return result;
    }
    
    [CanBeNull]
    public Actor GetTargetMostVisible(Vector3 position, Vector3 forward, Actor.Faction faction, float maxDistance = DEFAULT_SEARCH_DISTANCE)
    {
        Actor result = null;
        float closestDot = 0.0f;
        foreach (var actor in actors)
        {
            if (actor.faction != faction)
                continue;
            var diff = actor.transform.position - position;
            if (diff.sqrMagnitude > maxDistance * maxDistance)
                continue;
            var dir = diff.normalized;
            var dot = Vector3.Dot(dir, forward);
            if (dot < closestDot)
                continue;

            closestDot = dot;
            result = actor;
        }
        return result;
    }
}
