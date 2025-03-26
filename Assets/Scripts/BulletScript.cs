using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Security.Cryptography;

public class BulletScript : NetworkBehaviour
{
    public ulong originID;
    [SerializeField] private Rigidbody body;
    // Start is called before the first frame update

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsServer)
        {
            //initialization is happening on playershoot.cs now
            //InitializeBullet();
        }
    }

    public void InitializeBullet()
    {
        body.AddForce(transform.forward * 500f);
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsServer)
        {
            if (collision.rigidbody)
            {
                NetworkHealth possibleNetworkObject = collision.rigidbody.GetComponent<NetworkHealth>();

                if (possibleNetworkObject)
                {
                    possibleNetworkObject.DealDamage(originID);
                    ShowKillFeedRpc(originID, possibleNetworkObject.OwnerClientId);
                }
            }
            //Debug.Log(originID + " Shot " + collision.body.GetComponent<NetworkObject>().OwnerClientId);
            Destroy(gameObject);
        }
        
        //increase score
    }


    [Rpc(SendTo.NotServer)]
    public void ShowKillFeedRpc(ulong killer, ulong killed)
    {
        Debug.Log(killer + " Shot " + killed);
    }
}
