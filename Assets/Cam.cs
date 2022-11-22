using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public Actor target;
    public float distance = 10.0f;
    public float height = 3.0f;

    private void Update()
    {
        var targetPos = Vector3.zero;
        if (target != null)
        {
            targetPos = target.headPosition;
        }

        var camPos = targetPos;
        camPos.z -= distance;
        camPos.y += height;
        transform.position = camPos;
        transform.LookAt(targetPos + Vector3.up);
    }
}
