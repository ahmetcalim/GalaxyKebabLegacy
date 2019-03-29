using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.SceneManagement;
public class LavasGenerator : MonoBehaviour
{
    public GameObject lavasPrefab;
    public Transform lavasSpawnPoint;
    public static int generatedLavasCount;
    public GameObject currentLavas;
    public Transform arrivePoint;
    private bool canMove = false;
    public static bool lavasCanMove = true;
    public CircularDrive circularDrive;
    public void GenerateLavas()
    {
        
        if (generatedLavasCount == 0)
        {
            generatedLavasCount++;
            currentLavas = Instantiate(lavasPrefab, lavasSpawnPoint.position, Quaternion.identity);
        }
    }
    public void SiparisTeslim()
    {
        if (currentLavas != null)
        {
            StartCoroutine(Move());
        }
    }
    IEnumerator Move()
    {
        yield return new WaitForSeconds(.02f);

        if (currentLavas != null)
        {
            currentLavas.transform.position = Vector3.MoveTowards(currentLavas.transform.position, arrivePoint.position, 0.01f);
            if (currentLavas.transform.position != arrivePoint.position)
            {
                StartCoroutine(Move());
            }
        }
     
    }
    
}
