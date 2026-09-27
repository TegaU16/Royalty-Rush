using System;
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

    private bool isMoving;
    private bool isLeaving;

    private Book spawnedBook;

    public event Action OnCustomerLeave;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
    }

    public void Leave(bool angry)
    {
        isLeaving = true;
        splineAnimate.Container = CustomerManager.Instance.customerLeavePath;
        splineAnimate.NormalizedTime = 0f;
        splineAnimate.Play();

        isMoving = true;
        animator.SetBool(IsMovingHash, isMoving);

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
}
