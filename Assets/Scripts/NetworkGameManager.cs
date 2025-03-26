using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkGameManager : NetworkBehaviour
{
    [SerializeField] private Transform[] spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
        
    }

    public void RespawnPlayer(NetworkObject player)
    {
        //you can do a lot of stuff here
        //add delay
        
        Transform spawnpoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        player.transform.position = spawnpoint.position;
        player.transform.rotation = spawnpoint.rotation;

        player.GetComponent<NetworkHealth>().Revive();
    }
}
