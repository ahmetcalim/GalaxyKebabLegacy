using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerBehaviour : MonoBehaviour
{
    private void Start()
    {
        transform.LookAt(Camera.main.transform.position);
        StartCoroutine(Move());
    }
    IEnumerator Move()
    {
        yield return new WaitForSeconds(.1f);
        transform.position = Vector3.MoveTowards(transform.position, AlienSpawn.arrivePoint.position, 0.5f);
        StartCoroutine(Move());
    }
}
