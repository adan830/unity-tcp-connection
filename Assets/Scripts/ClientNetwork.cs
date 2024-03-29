﻿using UnityEngine;
using System.Collections;
 
public class ClientNetwork : MonoBehaviour {
 
    public string serverIP = "127.0.0.1";
    public int port = 25000;
    private string _messageLog = "";
    string someInfo = "";
    private NetworkPlayer _myNetworkPlayer;
 
    void OnGUI() {
        if (Network.peerType == NetworkPeerType.Disconnected) {
            if (GUI.Button(new Rect(100, 100, 150, 25), "Connect")) {
                Network.Connect(serverIP, port);
            }
        } else {
            if (Network.peerType == NetworkPeerType.Client) {
                GUI.Label(new Rect(100, 100, 150, 25), "client");
 
                if (GUI.Button(new Rect(100, 125, 150, 25), "Logout"))
                    Network.Disconnect();
 
                if (GUI.Button(new Rect(100, 150, 150, 25), "Send Hello to server"))
                    SendInfoToServer();
            }
        }
 
        GUI.TextArea(new Rect(250, 100, 300, 300), _messageLog);
    }
 
    [RPC]
    void SendInfoToServer(){
        someInfo = "Client " + _myNetworkPlayer.guid + ": hello server";
        networkView.RPC("ReceiveInfoFromClient", RPCMode.Server, someInfo);
    }
    [RPC]
    void SetPlayerInfo(NetworkPlayer player) {
        _myNetworkPlayer = player;
        someInfo = "Player setted";
        networkView.RPC("ReceiveInfoFromClient", RPCMode.Server, someInfo);
    }
 
    [RPC]
    void ReceiveInfoFromServer(string someInfo) {
        _messageLog += someInfo + "\n";
    }
 
    void OnConnectedToServer() {
        _messageLog += "Connected to server" + "\n";
    }
    void OnDisconnectedToServer() {
        _messageLog += "Disco from server" + "\n";
    }
 
    // fix RPC errors
    [RPC]
    void ReceiveInfoFromClient(string someInfo) { }
    [RPC]
    void SendInfoToClient(string someInfo) { }
}