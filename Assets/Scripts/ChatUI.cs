using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatUI : MonoBehaviour
{
    [SerializeField] TMP_InputField messageInputField;
    [SerializeField] TextMeshProUGUI messageDisplay;
    NetworkChat chat;

    public void InitializeChatUI(NetworkChat netChat)
    {
        chat = netChat;
        chat.OnMessageReceived += ReceiveMessageText;
    }

    // Start is called before the first frame update
    public void BTN_SendMessage()
    {
        string messageToSend = messageInputField.text;

        if (!chat)
        {
            InitializeChatUI(FindObjectOfType<NetworkChat>());
        }

        chat.SendMessageToChat(messageToSend);


    }
    public void ReceiveMessageText(string messageReceived)
    {
        messageDisplay.text += "\n" + messageReceived;
    }
}
