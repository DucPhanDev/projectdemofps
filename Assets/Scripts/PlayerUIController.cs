using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PathologicalGames;

public class PlayerUIController : SingletonMono<PlayerUIController>
{
    #region SerializeField Variables
    [SerializeField] private Text waveCountText;
    [SerializeField] private Text enemyCountText;
    [SerializeField] private Text textWinLose;
    [SerializeField] private Text textYourScore;
    [SerializeField] private Text textYourScoreEndMission;
    [SerializeField] private GameObject endMissionUI;
    [SerializeField] private GameObject guideRotateCamera;
    [SerializeField] private Camera camInGameUI;
    #endregion

    #region Private Variables
    private float rotationX = 0;
    private float rotationY = 0;
    private float halfScreenWidth;
    private int fingerLeftId;
    private int fingerRightId;
    private int yourScore = 0;
    private Vector2 lookInput;
    private Quaternion originalRotation;
    private bool onHoldShoot;
    #endregion

    #region Public Variables 
    public float cameraSensitivity;
    public float minimumX = -180f;
    public float maximumX = 180f;
    public float minimumY = -60f;
    public float maximumY = 60f;

    public Text currentAmmoText;
    public Rigidbody rgBody;
    #endregion

    void Start()
    {
        if (InGameManager.Instance.isFirstTimeStart)
            guideRotateCamera.SetActive(true);
        fingerLeftId = -1;
        fingerRightId = -1;
        halfScreenWidth = Screen.width / 2;

        if (rgBody)
            rgBody.freezeRotation = true;
        originalRotation = transform.localRotation;

        Setup();
    }
    public void Setup()
    {
        yourScore = 0;
        OnUpdateScore(0);
    }
    private void Update()
    {
        GetTouchInput();
        if (fingerLeftId != -1)
            LookAround();

        if (onHoldShoot)
            OnPressShoot();
    }
    public void OnHoldShoot(bool isHolding)
    {
        onHoldShoot = isHolding;
    }
    private void OnPressShoot()
    {
        Ray ray = camInGameUI.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        PlayerController.Instance.OnShoot(ray);
    }

    public void OnPressReload()
    {
        PlayerController.Instance.OnPressReload();
    }

    public void OnUpdateCurrentAmmoText(int currentAmmo)
    {
        currentAmmoText.text = currentAmmo.ToString();
    }

    public void OnUpdateScore(int score)
    {
        yourScore += score;
        if (yourScore <= 0)
            yourScore = 0;

        textYourScore.text = "Score: " + yourScore;
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
        InGameManager.Instance.isFirstTimeStart = false;
        guideRotateCamera.SetActive(false);
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

    public void OnUpdateEnemyCount(int enemyCount,int maxenemyCount)
    {
        enemyCountText.text = enemyCount + "/" + maxenemyCount;
    }

    public void OnUpdateWaveCount(int waveIndex)
    {
        waveCountText.text = "Wave " + (waveIndex + 1);
    }

    public void OnShowEndMissionUI(string text)
    {
        endMissionUI.SetActive(true);
        textWinLose.text = text;
        textYourScoreEndMission.text = textYourScore.text;
    }

    public void OnPressReplay()
    {
        endMissionUI.SetActive(false);
        Setup();
        InGameManager.Instance.Setup();
    }

}
