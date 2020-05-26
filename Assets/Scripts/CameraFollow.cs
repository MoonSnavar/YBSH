using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    void Update()
    {
        Vector3 targetPosition = target.TransformPoint(new Vector3(0, 1, -10));
        if (targetPosition.y < 0)
        {
            transform.position = Vector3.SmoothDamp(transform.position, new Vector3(0, 0, targetPosition.z), ref velocity, smoothTime);
        }
        else if (targetPosition.y >= 101.62f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, new Vector3(0, 101.62f, targetPosition.z), ref velocity, smoothTime);
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, new Vector3(0, targetPosition.y, targetPosition.z), ref velocity, smoothTime);
        }
    }
}
