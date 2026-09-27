using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    private Camera mainCam;
    private SplineAnimate splineAnimate;

    private Vector3 startPos;
    private Quaternion startRot;

    [SerializeField] private Vector3 gamePos;
    [SerializeField] private Quaternion gameRot;

    [SerializeField] private Transform bookCameraPoint;
    [SerializeField] private Quaternion bookCameraRot;

    [SerializeField] private float camMoveTime = 1f;
    [SerializeField] private float bookCamMoveTime = 0.5f;

    [SerializeField] private GameObject camToggleUp;
    [SerializeField] private GameObject camToggleDown;

    private bool isMoving;
    public bool InBookCamera { get; private set; }

    public event Action OnIntroComplete;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        mainCam = Camera.main;

        splineAnimate = mainCam.GetComponent<SplineAnimate>();

        splineAnimate.Completed += MoveCam;
        splineAnimate.Play();
    }

    private void OnDestroy()
    {
        if (splineAnimate != null)
            splineAnimate.Completed -= MoveCam;
    }

    private void MoveCam()
    {
        startPos = mainCam.transform.position;
        startRot = mainCam.transform.rotation;

        StartCoroutine(MoveCamRoutine());
    }

    private IEnumerator MoveCamRoutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < camMoveTime)
        {
            float t = Mathf.Clamp01(elapsedTime / camMoveTime);

            mainCam.transform.SetPositionAndRotation(
                Vector3.Lerp(startPos, gamePos, t),
                Quaternion.Lerp(startRot, gameRot, t)
            );

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCam.transform.SetPositionAndRotation(gamePos, gameRot);
        OnIntroComplete?.Invoke();
    }

    public void ToggleBookCamera(bool on)
    {
        if (isMoving) return;
        if ((InBookCamera && on == true) || (!InBookCamera && on == false)) return;

        StartCoroutine(BookCameraRoutine(on));
    }

    private IEnumerator BookCameraRoutine(bool on)
    {
        float elapsedTime = 0f;
        isMoving = true;

        while (elapsedTime < bookCamMoveTime)
        {
            float t = Mathf.Clamp01(elapsedTime / bookCamMoveTime);

            if (on)
            {
                mainCam.transform.SetPositionAndRotation(
                    Vector3.Lerp(gamePos, bookCameraPoint.position, t),
                    Quaternion.Lerp(gameRot, bookCameraRot, t)
                );
            }
            else
            {
                mainCam.transform.SetPositionAndRotation(
                    Vector3.Lerp(bookCameraPoint.position, gamePos, t),
                    Quaternion.Lerp(bookCameraRot, gameRot, t)
                );
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        camToggleUp.SetActive(!on);
        camToggleDown.SetActive(on);

        if (on)
            mainCam.transform.SetPositionAndRotation(bookCameraPoint.position, bookCameraRot);
        else
            mainCam.transform.SetPositionAndRotation(gamePos, gameRot);

        InBookCamera = on;
        isMoving = false;
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = mainCam.transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;

            mainCam.transform.localPosition = new Vector3(
                originalPos.x + x,
                originalPos.y + y,
                originalPos.z
            );

            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCam.transform.localPosition = originalPos;
    }
}
