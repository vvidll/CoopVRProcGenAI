using System.Text.RegularExpressions;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.NetworkingScripts
{
    public class NetworkManagerUI : MonoBehaviour
    {
        [SerializeField] string ipAddress = "127.0.0.1";
        [SerializeField] ushort port = 7777;

        [SerializeField] TMP_Text ipAddressText;
        [SerializeField] TMP_Text portText;

        [SerializeField] Button hostBtn;
        [SerializeField] Button clientBtn;

        [SerializeField] GameObject panelCoopInfo;

        [SerializeField] string gameScene = "GameScene";

        private void Awake()
        {
            if (hostBtn == null || clientBtn == null)
                Debug.LogError("References Buttons: Button (Host) or Button (Client) eguals null!");

            hostBtn.onClick.AddListener(() =>
            {
                SpawnHost();
            });

            clientBtn.onClick.AddListener(() =>
            {
                SpawnClient();
            });
        }

        public void SpawnHost() 
        {
            var utpTransport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;

            // spawn and connect host
            if (utpTransport)
            {
                utpTransport.SetConnectionData(Sanitize(ipAddressText.text), port);

                DisableWindowCoopInfo();
            }

            // if StartHost() equals true, switch the scene
            if (!NetworkManager.Singleton.StartHost())
            {
                Debug.LogError("Failed to start host");
            }

        }

        public void SpawnClient() 
        {
            var utpTransport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;

            // spawn and connect client
            if (utpTransport)
            {
                utpTransport.SetConnectionData(Sanitize(ipAddressText.text), port);

                Debug.Log("Local Client Id: " + NetworkManager.Singleton.LocalClientId);

                DisableWindowCoopInfo();
            }

            if (!NetworkManager.Singleton.StartClient())
            {
                Debug.LogError("Failed to start client");
            }
        }


        /// <summary>
        /// For clear address field, if user write not correct address
        /// </summary>
        /// <param name="dirtyString"></param>
        /// <returns></returns>
        static string Sanitize(string dirtyString)
        {
            return Regex.Replace(dirtyString, "[^A-Za-z0-9.]", "");
        }

        public void DisableWindowCoopInfo() => panelCoopInfo.SetActive(false);
    }
}