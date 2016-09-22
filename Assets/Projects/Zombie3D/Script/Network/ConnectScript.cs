using UnityEngine;
using System.Collections;

public class ConnectScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Network.Connect("192.168.2.103", 25000);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnConnectedToServer()
    {
        Debug.Log("Get Connected...");


        // Allow receiving data again
        Network.isMessageQueueRunning = true;
        // Now the level has been loaded and we can start sending out data
        Network.SetSendingEnabled(0, true);
    }

    void OnPlayerConnected(NetworkPlayer player)
    {
        Debug.Log("Player connected from " + player.ipAddress +":" + player.port);

    }

    void OnDisconnectedFromServer()
    {
        Debug.Log("DisConnected...");
    }
}
