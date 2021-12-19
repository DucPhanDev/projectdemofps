using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float rotationX = 0;
    private float rotationY = 0;
    private float halfScreenWidth;
    private int fingerLeftId;
    private int fingerRightId;
    private Vector2 lookInput;
    private Quaternion originalRotation;

    public float cameraSensitivity;
    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -60F;
    public float maximumY = 60F;
    
    public Rigidbody rgBody;

    void Start()
    {
        fingerLeftId = -1;
        fingerRightId = -1;
        halfScreenWidth = Screen.width / 2;

        if (rgBody)
            rgBody.freezeRotation = true;
        originalRotation = transform.localRotation;
    }

    private void Update()
    {
        GetTouchInput();
        if (fingerRightId != -1)
            LookAround();
    }

    private void GetTouchInput()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch t = Input.GetTouch(i);
            switch (t.phase)
            {
                case TouchPhase.Began:
                    if (t.position.x < halfScreenWidth && fingerLeftId == -1)
                    {
                        fingerLeftId = t.fingerId;
                    }
                    else if (t.position.x > halfScreenWidth && fingerRightId == -1)
                    {
                        fingerRightId = t.fingerId;
                    }
                    break;
                case TouchPhase.Moved:
                    if (t.fingerId == fingerRightId)
                    {
                        lookInput = t.deltaPosition;
                    }
                    break;
                case TouchPhase.Stationary:
                    if (t.fingerId == fingerRightId)
                    {
                        lookInput = Vector2.zero;
                    }
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (t.fingerId == fingerLeftId)
                    {
                        fingerLeftId = -1;
                    }
                    if (t.fingerId == fingerRightId)
                    {
                        fingerRightId = -1;
                    }
                    break;
            }
        }
    }
    private void LookAround()
    {
      
        rotationX += lookInput.x * cameraSensitivity*Time.deltaTime;
        rotationY += lookInput.y * cameraSensitivity * Time.deltaTime;
        rotationX = ClampAngle(rotationX, minimumX, maximumX);
        rotationY = ClampAngle(rotationY, minimumY, maximumY);
        Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);
        transform.localRotation = originalRotation * xQuaternion * yQuaternion;
    }
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
