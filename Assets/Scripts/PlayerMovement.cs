using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private GameObject chatPrefab;

    [SerializeField] private Rigidbody tankRigidbody;
    [SerializeField] private float movingSpeed = 2f;
    [SerializeField] private float rotationspeed = 3f;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsServer && IsLocalPlayer && IsOwner)
        {
            NetworkObject.InstantiateAndSpawn(chatPrefab, NetworkManager);
        }
        //if (IsLocalPlayer)
        //{
        //    FindObjectOfType<ChatUI>().InitializeChatUI();
        //}
    } 
        
    private void Update()
    {


        if (IsOwner)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            MoveTankRpc(horizontal, vertical);
        }
    }
    
    [Rpc(SendTo.Server)]
    public void MoveTankRpc(float horizontal, float vertical)
    {
        tankRigidbody.velocity = (transform.forward * vertical * Time.fixedDeltaTime * movingSpeed);
        //transform.Rotate(transform.up * horizontal * Time.deltaTime * rotationspeed);

        tankRigidbody.rotation = Quaternion.Euler(transform.eulerAngles + transform.up * horizontal * rotationspeed * Time.fixedDeltaTime);
    }
}


