using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class ClientServer : MonoBehaviour
{
    NetworkManager netManager;
    kcp2k.KcpTransport netTransport;
    bool gui_host, gui_client;
    string host_port, maxConnection, serverIP;

    [SerializeField] GameObject HostButton, ClientButton, SubmitButton, FTObject, STObject, TTObject, FIObject, SIObject, SFObject, SSObject, STrdObject, StopButton;
    [SerializeField] Text FirstText, SecondText, TitleText, SFText, SSText, STText;
    [SerializeField] InputField FirstInput, SecondInput;

    // Start is called before the first frame update
    void Start()
    {
        netManager = GetComponent<NetworkManager>();
        netTransport = GetComponent<kcp2k.KcpTransport>();
        gui_host = false;
        gui_client = false;
        host_port = "";
        maxConnection = "";
        serverIP = "";

        HostButton.SetActive(true);
        ClientButton.SetActive(true);
    }

    public void OnHostClick()
    {
        Debug.Log("Pressed Host");
        HostButton.SetActive(false);
        ClientButton.SetActive(false);
        gui_host = true;
        gui_client = false;
    }
    public void OnClientClick()
    {
        Debug.Log("Pressed Client");
        HostButton.SetActive(false);
        ClientButton.SetActive(false);
        gui_host = false;
        gui_client = true;
    }

    public void OnSubmitClick()
    {
        if (gui_host)
        {
            host_port = FirstInput.text;
            maxConnection = SecondInput.text;
            netManager.maxConnections = int.Parse(maxConnection);
            netTransport.Port = ushort.Parse(host_port);
            netManager.StartHost();
        }
        if (gui_client)
        {
            host_port = FirstInput.text;
            serverIP = SecondInput.text;
            netManager.networkAddress = serverIP;
            netTransport.Port = ushort.Parse(host_port);
            netManager.StartClient();
        }
    }

    public void OnStopClick()
    {
        HideIGUI();
        if (NetworkServer.active)
        {
            netManager.StopHost();
        }
        if (NetworkClient.active)
        {
            netManager.StopClient();
        }
    }

    void HideSetupUI()
    {
        TTObject.SetActive(false);
        FTObject.SetActive(false);
        STObject.SetActive(false);
        FIObject.SetActive(false);
        SIObject.SetActive(false);
        SubmitButton.SetActive(false);
    }

    void HideIGUI()
    {
        SFObject.SetActive(false);
        SSObject.SetActive(false);
        STrdObject.SetActive(false);
        StopButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gui_client && !gui_host)
        {
            HostButton.SetActive(true);
            ClientButton.SetActive(true);
        }
        else if (gui_host)
        {
            HideIGUI();
            TTObject.SetActive(true);
            FTObject.SetActive(true);
            STObject.SetActive(true);
            FIObject.SetActive(true);
            SIObject.SetActive(true);
            SubmitButton.SetActive(true);

            TitleText.text = "Host Setup";
            FirstText.text = "Host Port: ";
            SecondText.text = "Max Client: ";
            SubmitButton.GetComponentInChildren<Text>().text = "Start";
        }
        else if (gui_client)
        {

            HideIGUI();
            TTObject.SetActive(true);
            FTObject.SetActive(true);
            STObject.SetActive(true);
            FIObject.SetActive(true);
            SIObject.SetActive(true);
            SubmitButton.SetActive(true);

            TitleText.text = "Client Setup";
            FirstText.text = "Server Port: ";
            SecondText.text = "Server IP: ";
            SubmitButton.GetComponentInChildren<Text>().text = "Connect";
        }
        if (NetworkServer.active)
        {
            HideSetupUI();
            SFObject.SetActive(true);
            SSObject.SetActive(true);
            STrdObject.SetActive(true);
            StopButton.SetActive(true);

            SFText.text = "Host is Running";
            SSText.text = "Server IP: " + netManager.networkAddress;
            STText.text = "Server Port: " + netTransport.Port;
            StopButton.GetComponentInChildren<Text>().text = "Stop";
        }
        else if (NetworkClient.active)
        {
            HideSetupUI();
            SFObject.SetActive(true);
            SSObject.SetActive(false);
            STrdObject.SetActive(false);
            StopButton.SetActive(true);

            SFText.text = "Host is Running";
            StopButton.GetComponentInChildren<Text>().text = "Disconnect";
        }
    }

    /*private void OnGUI()
    {
        if (!netManager.isNetworkActive)
        {
            if (!gui_host && !gui_client)
            {
                if (GUI.Button(new Rect(10, 10, 130, 30), "Host"))
                {
                    gui_host = true;
                }
                if (GUI.Button(new Rect(10, 50, 130, 30), "Client"))
                {
                    gui_client = true;
                }
            }
            else if (gui_host)
            {
                GUI.Label(new Rect(10, 10, 200, 30), "Host Setup");
                GUI.Label(new Rect(10, 50, 120, 30), "Host Port: ");
                host_port = GUI.TextField(new Rect(135, 50, 100, 30), host_port);
                GUI.Label(new Rect(10, 90, 120, 30), "Max Client: ");
                maxConnection = GUI.TextField(new Rect(135, 90, 70, 30), maxConnection);
                if (GUI.Button(new Rect(10, 130, 120, 30), "Start Host"))
                {
                    netManager.maxConnections = int.Parse(maxConnection);
                    netTransport.Port = ushort.Parse(host_port);
                    netManager.StartHost();
                }
            }else if (gui_client)
            {
                GUI.Label(new Rect(10, 10, 200, 30), "Client Setup");
                GUI.Label(new Rect(10, 50, 120, 30), "Server Port: ");
                host_port = GUI.TextField(new Rect(135, 50, 100, 30), host_port);
                GUI.Label(new Rect(10, 90, 120, 30), "Server IP: ");
                serverIP = GUI.TextField(new Rect(135, 90, 70, 30), serverIP);
                if (GUI.Button(new Rect(10, 130, 120, 30), "Connect"))
                {
                    netManager.networkAddress = serverIP;
                    netTransport.Port = ushort.Parse(host_port);
                    netManager.StartClient();
                }
            }
        }
        
        else if(NetworkClient.active)
        {
            GUI.Label(new Rect(10, 10, 200, 30), "Client Connected");
            if (GUI.Button(new Rect(10, 130, 120, 30), "Disconnect"))
            {
                netManager.StopClient();
            }
        }
    }*/
}
