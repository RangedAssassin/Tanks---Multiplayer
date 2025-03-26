using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class NetworkHostSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject chatPrefab;
    [SerializeField] private GameObject gameManagerPrefab;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsServer && IsLocalPlayer && IsOwner)
        {
            NetworkObject.InstantiateAndSpawn(chatPrefab, NetworkManager);
            NetworkObject.InstantiateAndSpawn(gameManagerPrefab, NetworkManager);
        }
    }
}
