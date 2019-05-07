using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class PlayerPositionManager : MonoBehaviour
{
    public Transform player;
    public Transform playerCam;
    // Start is called before the first frame update
    void Start()
    {
       // player.position = new Vector3(-playerCam.position.x, player.localPosition.y, -playerCam.position.z) ;
        transform.position = new Vector3(playerCam.transform.position.x, 16.8f, playerCam.transform.position.z) ;
    }
    
}
