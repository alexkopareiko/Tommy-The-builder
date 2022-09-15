using System.Collections;
using System.Collections.Generic;
using System.Text;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Com.NikfortGames.MyGame 
{
	[RequireComponent(typeof(PhotonView))]
	public class RoomChat : MonoBehaviourPunCallbacks
	{

		#region Public Fields

		public static RoomChat instance;

		#endregion
		#region Private Fields

		[SerializeField] private List<KeyCode> m_sendKey;
		[SerializeField] private Text m_message = null;
		[SerializeField] private Transform m_content = null;
		[SerializeField] private InputField m_messageInput = null;

		private Queue<string> m_messageQueue = new Queue<string>();
		private StringBuilder m_messageBuilder = new StringBuilder();

		#endregion

		#region Message Limits

		[Header("Send Limitations")]

		//messages will be send every n seconds
		[SerializeField]
		private double SendRate = 1;

		//max capacity of the queue
		[SerializeField] private int QueueCapacity = 10;

		private bool m_canSend = true;
		private double m_nextSendingTime;

		#endregion

		#region MonoBehaviour Callbacks

		private void Awake() {
			instance = this;
			// m_messageInput.gameObject.CloseMenu();
		}

		private void Start()
		{
			m_messageInput.text = string.Empty;
			m_messageInput.readOnly = true;

			
		}

		private void Update() => PlayerInput();

		#endregion
		
		#region Private Fields

		private void PlayerInput()
		{
			bool keyPressed = false;
			foreach (var item in m_sendKey)
			{
				if(Input.GetKeyDown(item)) {
					keyPressed = true;
				}
			}
			if(!keyPressed) return;
			if (!m_messageInput.readOnly)
			{
				SendAndClose();
			}
			else
			{
				Open();
			}
		}

		private void SendAndClose()
		{
			//send message if available
			if (!string.IsNullOrEmpty(m_messageInput.text))
			{
				SendMessage();
			}

			//clean input Field
			m_messageInput.text = string.Empty;

			Close();
		}

		private void SendMessage()
		{
			if (m_canSend)
			{
				//Calculate next Time when the Message/s can be send
				//if less than 0 next out is 0
				var nextOut = m_nextSendingTime - Time.time < 0.0
					? 0.0
					: m_nextSendingTime - Time.time;
				HandleQueueLimit(m_messageInput.text);
				StartCoroutine(HandleMessageLimit(nextOut));
			}
			else
			{
				HandleQueueLimit(m_messageInput.text);
			}
		}

		/// <summary>
		/// Enqueue all Messages.
		/// If Message Queue is full stop adding and ignore Messages.
		/// </summary>
		/// <param name="msg">Message to Send.</param>
		private void HandleQueueLimit(string msg)
		{
			if (m_messageQueue.Count < QueueCapacity)
			{
				m_messageQueue.Enqueue(msg);
			}
			else
			{
				Debug.Log("Message Queue is full.Wait a moment");
			}
		}

		/// <summary>
		/// Calculate and set the Time to send a Message.
		/// "iterate" through all Messages in the Queue
		/// and stores them in as one large string.
		/// Send the large string via RPC.
		/// </summary>
		/// <returns></returns>
		private IEnumerator HandleMessageLimit(double delay)
		{
			m_canSend = false;
			m_nextSendingTime = Time.time + SendRate + delay;
			yield return new WaitForSeconds((float) delay);

			while (m_messageQueue.Count > 0)
			{
				var msg = m_messageQueue.Dequeue();
				m_messageBuilder.Append(msg);
				if (m_messageQueue.Count > 0)
				{
					m_messageBuilder.Append("\n");
				}
			}

			photonView.RPC(nameof(SendMessage), RpcTarget.All, m_messageBuilder.ToString());
			m_messageBuilder.Clear();
			m_canSend = true;
		}

		[PunRPC] 
		private void SendMessage(string text, PhotonMessageInfo info)
		{
			CreateLocalMessage(text, info.Sender);
		}

		private void CreateLocalMessage(string text, Photon.Realtime.Player sender)
		{
			var senderName = FormatName(sender);
			var messageText = Instantiate(m_message, m_content, false);

			messageText.text = senderName + " : " + text;
		}

		public void CreateLocalMessage(string text)
		{
			var messageText = Instantiate(m_message, m_content, false);

			messageText.text = text;
		}

		private string FormatName(Photon.Realtime.Player player)
		{
			var senderName = player.NickName;

			if (string.IsNullOrEmpty(player.NickName))
			{
				senderName = "Someone";
			}

			var localColor = ColorUtility.ToHtmlStringRGB(Constants.COLORS.GRAY);
			var otherColor = ColorUtility.ToHtmlStringRGB(Constants.COLORS.GREEN);
			var name = Equals(player, PhotonNetwork.LocalPlayer)
				? $"<color=#{localColor}> [ {senderName} ]</color>"
				: $"<color=#{otherColor}> [ {senderName} ]</color>";
			return name;
		}

		#endregion

		#region Public Methods

		public void OnPointerEnter() {
		}

		public void OnPointerExit() {

		}

		public void OnPointerDown()
		{

		}

		public void Open()
		{
			if(m_messageInput.readOnly) {
				//set focus
				m_messageInput.readOnly = false;
				EventSystem.current.SetSelectedGameObject(m_messageInput.gameObject);
				m_messageInput.Select();
				GameManager.instance.gameIsPaused = true;
			}
		}

		public void Close() {
			if(!m_messageInput.readOnly) {
				var eventSystem = EventSystem.current;
				if (!eventSystem.alreadySelecting) eventSystem.SetSelectedGameObject (null);
				// EventSystem.current.SetSelectedGameObject(null);
				m_messageInput.readOnly = true;
				GameManager.instance.gameIsPaused = false;
			}
			
		}


		#endregion


	}
}