using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    private Vector3 offset = new Vector3(0f, 0f, -100f);
    private Vector3 velocity = Vector3.zero;
    private float smoothTime = 0.25f;
    private float minCameraHeight = -5f;

    // Update is called once per frame
    private void FixedUpdate()
    {      
        if (playerTransform.position.y < minCameraHeight)
        {
            Vector3 newPlayerPosition = new Vector3(playerTransform.position.x, minCameraHeight, playerTransform.position.z) + offset;
            transform.position = Vector3.SmoothDamp(transform.position, newPlayerPosition, ref velocity, smoothTime);
        }
        else
        {
            Vector3 playerPosition = playerTransform.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, playerPosition, ref velocity, smoothTime);
        }            
    }
}
