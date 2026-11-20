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

        [SerializeField] string gameScene = "GameScene";

        private void Awake()
        {
            if (hostBtn == null || clientBtn == null)
                Debug.LogError("References Buttons: Button (Host) or Button (Client) eguals null!");

            hostBtn.onClick.AddListener(() =>
            {
                StartGame();
            });

            clientBtn.onClick.AddListener(() =>
            {
                JoinGame();
            });
        }

        public void StartGame()
        {
            var utpTransport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;

            // spawn and connect host
            if (utpTransport)
            {
                utpTransport.SetConnectionData(Sanitize(ipAddressText.text), port);
            }

            // if StartHost() equals true, switch the scene
            if (NetworkManager.Singleton.StartHost())
            {
                SceneTransition.Instance.RegisterCallbacks();
                SceneTransition.Instance.SwitchScene(gameScene);
            }

            // if host not connect
            else
            {
                Debug.LogError("Failed to start host");
            }
        }

        public void JoinGame()
        {
            var utpTransport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;

            // spawn and connect client
            if (utpTransport)
            {
                utpTransport.SetConnectionData(Sanitize(ipAddressText.text), port);
            }

            // if StartClient() equals false, client not connect
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
    }
}