using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Chat : NetworkBehaviour
{
    [SerializeField] GameObject Chat_UI;
    [SerializeField] Text Chat_Text;
    [SerializeField] InputField Input_Text;

    private static event Action<string> OnMessage;

    public override void OnStartAuthority()
    {
        Chat_UI.SetActive(true);
        OnMessage += HandleNewMessage;
    }

    void HandleNewMessage(string message)
    {
        Chat_Text.text += message;
    }

    [Client]
    public void Send()
    {
        string message = Input_Text.text;
        Debug.Log("Sending ''" + message + "''" );
        if (string.IsNullOrWhiteSpace(message)) { return; }
        CmdSendMessage(Input_Text.text);
        Input_Text.text = string.Empty;
    }

    [Command]
    void CmdSendMessage(string message)
    {
        Debug.Log("Cmd: " + connectionToClient.connectionId + ": " + message);
        
        RpcSendMessage($"[{connectionToClient.connectionId}]: {message}");
    }

    [ClientRpc]
    void RpcSendMessage(string message)
    {
        Debug.Log("Rpc: " + Chat_Text.name);
        OnMessage?.Invoke($"\n{message}");
    }
}
