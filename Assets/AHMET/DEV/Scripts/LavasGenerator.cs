using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class LavasGenerator : MonoBehaviour
{
    public Transform kitchen;
    public GameObject lavasPrefab;
    public Transform lavasSpawnPoint;
    public static int generatedLavasCount;
    public GameObject currentLavas;
    public Transform arrivePoint;
    private bool canMove = false;
    public static bool lavasCanMove = true;
    public CircularDrive circularDrive;
    public HoverButton hoverButton;
    public static UnityEvent onLavasArrived;
    public LinearDrive linearDrive;
    private void Awake()
    {
        linearDrive.onEndPoint.AddListener(SiparisTeslim);
    }
    public void GenerateLavas()
    {
        if (generatedLavasCount == 0)
        {
            generatedLavasCount++;
            currentLavas = Instantiate(lavasPrefab, lavasSpawnPoint.position, Quaternion.identity);
            currentLavas.transform.SetParent(kitchen);
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
            currentLavas.transform.position = Vector3.MoveTowards(currentLavas.transform.position, arrivePoint.position, 0.03f);
            if (currentLavas.transform.position != arrivePoint.position)
            {
                StartCoroutine(Move());
            }
            else
            {
                if (!hoverButton.enabled)
                {
                    Destroy(currentLavas);
                    generatedLavasCount = 0;
                    GenerateLavas();
                }
                hoverButton.enabled = true;
            }
        }
     
    }
    
}
