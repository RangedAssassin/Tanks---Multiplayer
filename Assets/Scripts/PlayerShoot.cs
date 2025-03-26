using UnityEngine;
using Unity.Netcode;

public class PlayerShoot : NetworkBehaviour
{
    [SerializeField] private BulletScript bulletPrefab;
    [SerializeField] private Transform weaponTip;
    //[SerializeField] private int bulletSpeed = 500;

    private void Update()
    {
        if (IsOwner)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SpawnBulletRpc();

                #region Alt Options
                //Alternate Option
                //NetworkManager.SpawnManager.InstantiateAndSpawn(); use instead of instantiate
                //NetworkObject.InstantiateAndSpawn(); use instead of instantiate
                //NetworkObject.InstantiateAndSpawn(bulletPrefab.gameObject, NetworkManager);

                //Offline Option
                //Rigidbody bulletClone = Instantiate(bulletPrefab, weaponTip.position, weaponTip.rotation);
                //bulletClone.AddForce(weaponTip.forward * bulletSpeed);
                #endregion
            }
        }
        
    }

    [Rpc(SendTo.Server)]
    public void SpawnBulletRpc(RpcParams param = default)
    {
        Debug.Log(param.Receive.SenderClientId);

        BulletScript bulletClone = Instantiate(bulletPrefab, weaponTip.position, weaponTip.rotation);
        bulletClone.NetworkObject.Spawn();
        bulletClone.InitializeBullet();
        bulletClone.originID = param.Receive.SenderClientId;
        
    }
    
}
