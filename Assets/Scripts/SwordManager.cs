using System.Collections;
using Game.Audio;
using TMPro;
using UnityEngine;
using UnityEngine.Splines;

public class SwordManager : MonoBehaviour
{
    private static readonly WaitForSeconds _waitForSeconds0_8 = new(0.8f);
    public GameObject sword;
    public KeyCode swordKey = KeyCode.Q;

    public int charges = 3;
    public TextMeshProUGUI chargeText;

    private bool isSwinging;

    private SplineAnimate splineAnimate;

    public AudioClip swordWhoosh;

    // Start is called once before the first execution of Update after tis created
    void Start()
    {
        chargeText.text = charges.ToString();
        splineAnimate = sword.GetComponent<SplineAnimate>();
        splineAnimate.Completed += DisableSword;
        GameManager.Instance.OnReady += SpawnSword;
    }

    private void OnDestroy()
    {
        splineAnimate.Completed -= DisableSword;
        GameManager.Instance.OnReady -= SpawnSword;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.IsReady) return;

        if (Input.GetKeyDown(swordKey))
            TrySwing();
    }

    private void TrySwing()
    {
        if (isSwinging) return;

        if (charges <= 0)
        {
            StartCoroutine(GameManager.Instance.Charges());
            return;
        }

        if (CameraController.Instance.InBookCamera)
        {
            StartCoroutine(GameManager.Instance.BookModeSwing());
            return;
        }

        splineAnimate.NormalizedTime = 0f;
        sword.SetActive(true);
        splineAnimate.Play();
        isSwinging = true;

        StartCoroutine(PlayWhoosh());

        charges--;
        chargeText.text = charges.ToString();
    }

    private IEnumerator PlayWhoosh()
    {
        yield return _waitForSeconds0_8;
        AudioManager.Instance.PlaySFX(swordWhoosh);

        CustomerManager.Instance.KillCustomer();
    }


    private void SpawnSword() => sword.SetActive(true);

    private void DisableSword()
    {
        sword.SetActive(false);
        isSwinging = false;
    }
}
