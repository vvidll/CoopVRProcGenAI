using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.NetworkingScripts
{
    public class SceneTransition : MonoBehaviour
    {
        public static SceneTransition Instance { get; private set; }
        public event Action<ulong> OnClientLoadedScene;

        /// <summary>
        /// if another version exists, destroy it and use the current version 
        /// </summary>
        private void Awake()
        {
            if (Instance != this && Instance != null)
            {
                Destroy(Instance.gameObject);
            }

            Instance = this;

            DontDestroyOnLoad(this);
        }

        public void RegisterCallbacks()
        {
            NetworkManager.Singleton.SceneManager.OnLoadComplete += OnLoadComplete;
        }

        public void SwitchScene(string sceneName)
        {
            if (NetworkManager.Singleton.IsListening)
                NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);

            else
                SceneManager.LoadSceneAsync(sceneName);
        }

        public void OnLoadComplete(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
        {
            OnClientLoadedScene?.Invoke(clientId);
        }
    }
}