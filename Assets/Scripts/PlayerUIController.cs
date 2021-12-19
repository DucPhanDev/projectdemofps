using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;

public class PlayerUIController : SingletonMono<PlayerUIController>
{
    #region SerializeField Variables
    [SerializeField] private RectTransform rectUI;
    [SerializeField] private Transform bulletSpawnPos;
    [SerializeField] private Transform bulletTrans;
    [SerializeField] private Camera camInGameUI;
    #endregion

    #region Private Variables
    private float rotationX = 0;
    private float rotationY = 0;
    private float halfScreenWidth;
    private int fingerLeftId;
    private int fingerRightId;
    private Vector2 lookInput;
    private Quaternion originalRotation;
    private Vector3 worldPoint;
    private Vector3 viewPort;
    #endregion

    #region Public Variables 
    public float cameraSensitivity;
    public float minimumX = -180f;
    public float maximumX = 180f;
    public float minimumY = -60f;
    public float maximumY = 60f;

    public Rigidbody rgBody;
    #endregion

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
        if (fingerLeftId != -1)
            LookAround();

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 screenPoint = camInGameUI.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0));
            RectTransformUtility.ScreenPointToWorldPointInRectangle(rectUI, screenPoint, camInGameUI, out worldPoint);

            Transform go = PoolManager.Pools["BulletPlayer_Pool"].Spawn(bulletTrans, bulletSpawnPos.position, Quaternion.identity);
            go.SetParent(null);
            go.GetComponent<BulletPlayer>().Setup();
            go.LookAt(worldPoint);
        }
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
                    if (t.fingerId == fingerLeftId)
                    {
                        lookInput = t.deltaPosition;
                    }
                    break;
                case TouchPhase.Stationary:
                    if (t.fingerId == fingerLeftId)
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

        rotationX += lookInput.x * cameraSensitivity * Time.deltaTime;
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
