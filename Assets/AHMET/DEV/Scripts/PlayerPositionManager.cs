using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class PlayerPositionManager : MonoBehaviour
{
    public Transform playerSpawnPoint;
    public Transform player;
    public Transform playerCam;
    // Start is called before the first frame update
    void Start()
    {
        player.localPosition = new Vector3(-playerCam.position.x, player.localPosition.y, -playerCam.position.z) ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
