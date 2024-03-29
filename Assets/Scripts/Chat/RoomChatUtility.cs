﻿using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Com.NikfortGames.MyGame  {
	[RequireComponent(typeof(RoomChat))]
	public class RoomChatUtility : MonoBehaviourPunCallbacks
	{
		[SerializeField] private RoomChat m_roomChat = null;

		public override void OnJoinedRoom()
		{
			var colorCode = ColorUtility.ToHtmlStringRGB(Constants.COLORS.PURPLE);
			m_roomChat.CreateLocalMessage($"<color=#{colorCode}>You joined the Game. </color>");
		}

		public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
		{
			var colorCode = ColorUtility.ToHtmlStringRGB(Constants.COLORS.PURPLE);
			m_roomChat.CreateLocalMessage($"<color=#{colorCode}>{newPlayer.NickName} joined the Game. </color>");
		}

		public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
		{
			var colorCode = ColorUtility.ToHtmlStringRGB(Constants.COLORS.PURPLE);
			m_roomChat.CreateLocalMessage($"<color=#{colorCode}>{otherPlayer.NickName} left the Game.</color>");
		}

	}
}
