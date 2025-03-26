using UnityEngine;
using Unity.Netcode;


public class NetworkHealth : NetworkBehaviour
{
    [SerializeField] private int defaultHealth = 3;
    [SerializeField] private NetworkObject deathEffects;

    public NetworkVariable<int> health = new NetworkVariable<int>(readPerm: NetworkVariableReadPermission.Everyone,
        writePerm: NetworkVariableWritePermission.Server);

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if(IsServer)
        {
            health.Value = defaultHealth;
        }
        else
        {
            health.OnValueChanged += OnHealthZero;//client is listening for changes on the value
        }
    }

    private void OnHealthZero(int previous, int current)
    {
        //change health bar value here
        //this is happening for every client

        if (current <= 0) //client is checking if current value of life is 0
        {
            //start a UI Effect
            Debug.Log("I'm Dead");
        }
    }

    public void DealDamage(ulong damageOrigin)
    {
        health.Value--;


        if (health.Value <= 0)
        {
            //Die();
            FindObjectOfType<NetworkGameManager>().RespawnPlayer(NetworkObject);

            NetworkObject effect = Instantiate(deathEffects, transform.position + Vector3.up, Quaternion.identity);
            effect.Spawn();
            Destroy(effect.gameObject, 3f);

        }
        Debug.Log(damageOrigin + " Shot " + OwnerClientId);
            
    }

    public void Revive()
    {
        health.Value = defaultHealth;
    }
}
