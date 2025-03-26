using Unity.Collections;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class PlayerInfo : NetworkBehaviour
{
    [SerializeField] TextMeshPro nicknameDisplay;
    public NetworkVariable<FixedString32Bytes> playerNickname = 
        new NetworkVariable<FixedString32Bytes>(writePerm: NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        playerNickname.OnValueChanged += UpdateNickname;
        
        if (IsLocalPlayer)
        {
            playerNickname.Value = FindObjectOfType<UINetworkManager>().nicknameInputField.text;
        }

        nicknameDisplay.text = playerNickname.Value.ToString();

    }

    private void UpdateNickname(FixedString32Bytes previous, FixedString32Bytes current)
    { 
        nicknameDisplay.text = current.ToString();
    }
}
