using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;

namespace Services
{
    public class NetService : MonoBehaviourPunCallbacks
	{
		private enum NetworkStatus
		{
			Disconnected,
			WaitingForOpponent,
			Loading,
			LoadingDone,
			Playing
		}

		//TODO: Add support for more rooms, so more than 1 group can play at a given time.
		private const string ROOM_NAME = "HighNoon";

		public string PlayerId { get { return PhotonNetwork.LocalPlayer.UserId; } }
		public bool IsMaster 
		{ 
			get
			{ 
				return (m_netStatus == NetworkStatus.Disconnected) || PhotonNetwork.LocalPlayer.IsMasterClient; 
			} 
		}
		public bool IsInitialized { get{ return m_isInitialized; } set{} }

		private bool m_isInitialized;
		private NetworkStatus m_netStatus;

		private Action m_onStartGameLoad;
		private Action m_onStartGameReady;

		public void Awake()
		{
			if (Service.Network == null)
			{
				Service.Network = this;
			}
		}

		//Connect to the server and wait for an opponent to connect.
		public void Connect(Action startBattleCallback)
		{
			if(!m_isInitialized)
			{
				//Serialize player team
				PhotonNetwork.LogLevel = PunLogLevel.ErrorsOnly;
                PhotonNetwork.AutomaticallySyncScene = true;
				m_onStartGameLoad = startBattleCallback;
				m_netStatus = NetworkStatus.WaitingForOpponent;
				m_isInitialized = true;
                PhotonNetwork.ConnectUsingSettings();
			}
		}

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to master");
            m_netStatus = NetworkStatus.Loading;

            RoomOptions opts = new RoomOptions();
            opts.MaxPlayers = 2;

            TypedLobby lobby = new TypedLobby(ROOM_NAME, LobbyType.Default);

            PhotonNetwork.JoinOrCreateRoom(ROOM_NAME, opts, lobby);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log("Join Room Failed: " + message);
        }

        public override void OnJoinedRoom()
        {
            m_netStatus = NetworkStatus.Playing;
            Debug.Log("Room joined");
            if (m_onStartGameLoad != null)
            {
                m_onStartGameLoad();
            }
        }

        //This is the last method called at the end of the update.
        public void Update()
		{
		}

		public void Disconnect()
		{
			PhotonNetwork.Disconnect();

			//PhotonNetwork.OnEventCall -= OnNetworkEvent;

			m_isInitialized = false;
			m_netStatus = NetworkStatus.Loading;
			
			//opponents = null;
			
			m_onStartGameLoad = null;
			m_onStartGameReady = null;
		}
	}
}

