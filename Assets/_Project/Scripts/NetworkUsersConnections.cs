using System.Collections.Generic;
using _Project.Scripts.NetworkingScripts;
using Unity.Netcode;
using UnityEngine;


[RequireComponent(typeof(NetworkObject))] // for NetworkBehaviour
public class NetworkUsersConnections : NetworkBehaviour
{
    [SerializeField] NetworkManagerUI networkManagerUI;

    public Transform[] pointsConnectionsUsers;

    [SerializeField] NetworkObject playerPrefab;

    /// <summary>
    /// if connect host or client
    /// </summary>
    public override void OnNetworkSpawn() // почему-то клиент не подписывается на событие
                                          // и из-за этого у клиента не отображается правильно позиция хоста и его же самого
                                          // у хоста же все замечательно, если подключается к нему клиент,
                                          // то у хоста и клиента позиция правильная                     
    {
        if (NetworkManager.Singleton != null)
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        
    }
    
    public void OnClientConnected(ulong clientId) 
    {
        int spawnIndex = 0;

        if (!NetworkManager.Singleton.IsServer) return;

        // spawn client in remaining points
        if (clientId != NetworkManager.Singleton.LocalClientId) 
        {
            spawnIndex = NetworkManager.Singleton.ConnectedClientsIds.Count - 1;
        }

        if (spawnIndex < pointsConnectionsUsers.Length)
        {
            Transform spawnPoint = pointsConnectionsUsers[spawnIndex];

            NetworkObject playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);

            playerInstance.SpawnAsPlayerObject(clientId, destroyWithScene: false);
        }

        else 
        {
            Debug.LogError("Lobby is fulled!");
        }
    }

    /// <summary>
    /// if disconnect host or client
    /// </summary>
    public override void OnNetworkDespawn()
    {
        if (NetworkManager.Singleton != null)
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
    }
}
