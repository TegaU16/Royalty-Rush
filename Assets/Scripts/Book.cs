using System.Collections;
using Game.Audio;
using UnityEngine;
using UnityEngine.Splines;

public class Book : MonoBehaviour
{
    private static readonly WaitForSeconds _waitForSeconds0_5 = new(0.5f);

    [SerializeField] private GameObject topCover;
    [SerializeField] private Transform bookHinge;
    [SerializeField] private float rotationSpeed = 50f;

    [SerializeField] private PaymentUI paymentUI;
    [SerializeField] private SplineAnimate splineAnimate;

    public AudioClip kaching;

    private Customer customer;

    private readonly float targetRot = 180f;

    private bool isOpen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(OpenRoutine());
        splineAnimate.Container = CustomerManager.Instance.bookDiscardPath;
        splineAnimate.Completed += RemoveBook;
    }

    private IEnumerator OpenRoutine()
    {
        yield return _waitForSeconds0_5;

        float currentRot = 0f;

        Payment payment = PaymentGenerator.Instance.GeneratePayment(CustomerManager.Instance.CurrentDifficulty);
        paymentUI.SetPayment(payment);

        while (!isOpen)
        {
            if (currentRot >= targetRot)
            {
                isOpen = true;
                yield break;
            }

            float rotation = rotationSpeed * Time.deltaTime;

            topCover.transform.RotateAround(
                bookHinge.position,
                Vector3.forward,
                rotation
            );

            currentRot += rotation;

            yield return null;
        }
    }

    private IEnumerator CloseRoutine()
    {
        float currentRot = 0f;

        while (isOpen)
        {
            if (currentRot >= targetRot)
            {
                isOpen = false;
                splineAnimate.NormalizedTime = 0f;
                splineAnimate.Play();
                yield break;
            }

            topCover.transform.RotateAround(bookHinge.position, Vector3.forward, -rotationSpeed * Time.deltaTime);
            currentRot += rotationSpeed * Time.deltaTime;
            yield return null;
        }
    }

    public void FinishPayment(bool paymentCorrect, Payment payment)
    {
        AudioManager.Instance.PlaySFX(kaching);
        CameraController.Instance.ToggleBookCamera(on: false);
        StartCoroutine(CloseRoutine());

        if (paymentCorrect)
            customer.Leave(false, payment);
        else
            customer.Leave(true, payment);
    }

    private void RemoveBook() => Destroy(gameObject);

    public void SetCustomer(Customer customer) => this.customer = customer;

    public void Discard()
    {
        if (isOpen)
        {
            StartCoroutine(CloseRoutine());
            return;
        }

        StopAllCoroutines();
        splineAnimate.NormalizedTime = 0f;
        splineAnimate.Play();
    }
}
