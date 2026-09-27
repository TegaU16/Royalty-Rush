using System;
using System.Collections;
using Game.Audio;
using Game.UI;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(Animator))]
public class Customer : MonoBehaviour
{
    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
    private static readonly int PlaceHash = Animator.StringToHash("Place");

    [SerializeField] private SplineAnimate splineAnimate;
    [SerializeField] private Animator animator;

    [SerializeField] private GameObject book;
    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private AudioClip slamSound;

    [SerializeField] private PatienceBar patienceBar;
    private CanvasGroup patienceBarCG;
    private RectTransform patienceRect;

    public int patience = 20;
    private float currentPatience;

    public ParticleSystem deathPS;

    private bool isMoving;
    private bool isLeaving;

    private Book spawnedBook;

    public event Action OnCustomerLeave;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentPatience = patience;

        patienceBar.SetPatience(currentPatience, patience);
        patienceBarCG = patienceBar.gameObject.GetComponent<CanvasGroup>();
        patienceRect = patienceBar.gameObject.GetComponent<RectTransform>();

        splineAnimate.Completed += PlaceBook;
    }

    private void OnDestroy()
    {
        splineAnimate.Completed -= PlaceBook;
    }

    private void PlaceBook()
    {
        if (isLeaving) return;

        isMoving = false;
        animator.SetBool(IsMovingHash, isMoving);
        animator.SetTrigger(PlaceHash);
    }

    private void SpawnBook()
    {
        Transform spawnPoint = CustomerManager.Instance.bookSpawnPoint;
        GameObject bookObject = Instantiate(book, spawnPoint);

        spawnedBook = bookObject.GetComponent<Book>();
        spawnedBook.SetCustomer(this);

        CustomerManager.Instance.PlayImpact();

        AudioManager.Instance.PlaySFX(slamSound);
        StartCoroutine(Patience());
        StartCoroutine(CameraController.Instance.Shake(duration: 0.5f, magnitude: 0.04f));
    }

    private IEnumerator Patience()
    {
        UITween.PopIn(patienceRect, patienceBarCG);

        while (GameManager.Instance.IsReady && currentPatience > 0f && !isLeaving)
        {
            currentPatience -= Time.deltaTime;

            patienceBar.SetPatience(currentPatience, patience);

            yield return null;
        }

        if (!isLeaving && currentPatience <= 0f)
        {
            currentPatience = 0f;

            patienceBar.SetPatience(currentPatience, patience);
            if (spawnedBook != null)
                spawnedBook.Discard();
            Leave(angry: true, null);
        }
    }

    public void Leave(bool angry, Payment payment)
    {
        if (isLeaving) return;
        isLeaving = true;

        UITween.FadeOut(patienceBarCG);

        if (angry)
            GameStats.Instance.CustomerAngry();
        else
            GameStats.Instance.CustomerSatisfied(payment);

        splineAnimate.Container = CustomerManager.Instance.customerLeavePath;
        splineAnimate.NormalizedTime = 0f;
        splineAnimate.Play();

        isMoving = true;
        animator.SetBool(IsMovingHash, isMoving);

        StopAllCoroutines();
        OnCustomerLeave?.Invoke();
        Destroy(gameObject, 8f);
    }

    public void SetSplineContainer(SplineContainer splineContainer)
    {
        splineAnimate.Container = splineContainer;
        splineAnimate.Play();

        isMoving = true;
        animator.SetBool(IsMovingHash, isMoving);
    }

    public void PlayFootstepSound()
    {
        int index = UnityEngine.Random.Range(0, footstepSounds.Length);
        AudioClip clip = footstepSounds[index];

        AudioManager.Instance.PlaySFX(clip);
    }

    public void Die()
    {
        StopAllCoroutines();

        UITween.FadeOut(patienceBarCG);

        if (spawnedBook != null)
            spawnedBook.Discard();

        deathPS.transform.SetParent(null);
        deathPS.Play();

        Destroy(deathPS.gameObject, 2f);
        Destroy(gameObject, 0.5f);
    }
}
