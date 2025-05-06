using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using Unity.Collections;
public class PlayerInfo : NetworkBehaviour
{
    [SerializeField] private TextMeshPro nicknameDisplay;
    public NetworkVariable<FixedString32Bytes> playerNickname = new NetworkVariable<FixedString32Bytes>(writePerm: NetworkVariableWritePermission.Owner);
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        playerNickname.OnValueChanged += UpdateNickname;

        if (IsLocalPlayer) //IF I AM LOCAL PLAYER
        {
            //Get the nickname locally
            playerNickname.Value = FindObjectOfType<UINetworkManager>().nicknameInputField.text;
        }


        nicknameDisplay.text = playerNickname.Value.ToString(); //Display on UI
    }

    private void UpdateNickname(FixedString32Bytes previous, FixedString32Bytes current)
    {
        //Display on UI
        nicknameDisplay.text = current.ToString();
    }
}