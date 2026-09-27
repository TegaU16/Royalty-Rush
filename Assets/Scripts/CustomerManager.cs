using UnityEngine;
using UnityEngine.Splines;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager Instance;

    [SerializeField] private GameObject[] customerPrefabs;

    public SplineContainer customerPath;
    public SplineContainer customerLeavePath;
    public SplineContainer bookDiscardPath;
    public Transform bookSpawnPoint;

    private Customer activeCustomer;

    public int CurrentDifficulty { get; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.IsReady) return;

        if (activeCustomer == null)
            SpawnCustomer();
    }

    private void SpawnCustomer()
    {
        int index = Random.Range(0, customerPrefabs.Length);
        GameObject customerPrefab = customerPrefabs[index];

        GameObject customerObj = Instantiate(customerPrefab, customerPath.EvaluatePosition(0), Quaternion.identity);
        activeCustomer = customerObj.GetComponent<Customer>();

        activeCustomer.SetSplineContainer(customerPath);
        activeCustomer.OnCustomerLeave += RemoveCustomer;
    }

    private void RemoveCustomer() => activeCustomer = null;
}
