using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
       
    private Func<Vector3> GetCameraFollowPositionFunc;
    private Camera myCamera;


    // To handle the camera movement close to walls
    public bool isNearWall;

    public void Setup(Func<Vector3> GetCameraFollowPositionFunc)
    {
        this.GetCameraFollowPositionFunc = GetCameraFollowPositionFunc;
    }

    private void Start()
    {
        myCamera = transform.GetComponent<Camera>();
    }


    // Update is called once per frame
    void Update()
    {
        if (!isNearWall)
        {
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        Vector3 cameraFollowPosition = GetCameraFollowPositionFunc();
        cameraFollowPosition.z = transform.position.z;

        Vector3 cameraMoveDir = (cameraFollowPosition - transform.position).normalized;
        float distance = Vector3.Distance(cameraFollowPosition, transform.position);
        float cameraMoveSpeed = 10f;

        if (distance > 0)
        {
            Vector3 newCameraPosition = transform.position + cameraMoveDir * distance * cameraMoveSpeed * Time.deltaTime;

            // To prevent the Camera Overshooting the player on low framerate
            float distanceAfterMoving = Vector3.Distance(newCameraPosition, cameraFollowPosition);

            if (distanceAfterMoving > distance)
            {
                //OverShot the target
                newCameraPosition = cameraFollowPosition;
            }

            transform.position = newCameraPosition;
        }
    }

    public void HandleZoom(float cameraZoom)
    {
        float cameraZoomDiference = cameraZoom - myCamera.orthographicSize;
        float cameraZoomSpeed = 1f;
        myCamera.orthographicSize += cameraZoomDiference * cameraZoomSpeed * Time.deltaTime;
    }
}
