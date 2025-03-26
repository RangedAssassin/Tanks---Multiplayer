using UnityEngine;
using Unity.Netcode;
using System;

public class NetworkChat : NetworkBehaviour
{
    public Action<string> OnMessageReceived;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        FindObjectOfType<ChatUI>().InitializeChatUI(this);
    }
    public void SendMessageToChat(string messageReceived)
    {
        string whoSentThis = "Client";
        if (IsOwner)
        {
            whoSentThis = "Host";
        }
        SendMessageRpc(messageReceived, whoSentThis);
    }

    [Rpc(SendTo.Everyone)]
    public void SendMessageRpc(string messageReceived, string username)
    {
       OnMessageReceived?.Invoke(username + ":" + messageReceived);
    }
}
