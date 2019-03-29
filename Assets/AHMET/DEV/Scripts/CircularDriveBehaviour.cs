using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
public class CircularDriveBehaviour : MonoBehaviour
{
    public GameObject sphere;
    private float point;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float a = (GetComponent<CircularDrive>().outAngle / 360f);
        if ( a > point)
        {
            sphere.transform.Translate(Vector3.right * Time.deltaTime, Space.World);
        }
        else if (a < point)
        {
            sphere.transform.Translate(Vector3.left * Time.deltaTime, Space.World);
        }
        else if (Mathf.Abs(a - point) <=0.1f)
        {
            Debug.Log("sssss");
        }
        point = a;
    }
}
