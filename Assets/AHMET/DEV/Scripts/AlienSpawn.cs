using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienSpawn : MonoBehaviour
{
    public List<GameObject> customers;
    public Transform spawnPoint;
    public GameObject currentCustomer;
    public Transform _arrivePoint;
    public static Transform arrivePoint;
    private void Start()
    {
        arrivePoint = _arrivePoint;
    }
    private int GetRandomIndex()
    {
        return Random.Range(0, 3);
    }
    public void SpawnCustomer()
    {
        currentCustomer = Instantiate(customers[GetRandomIndex()], spawnPoint.position, Quaternion.identity);
    }
}
