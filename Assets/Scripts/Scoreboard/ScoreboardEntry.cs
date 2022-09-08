using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class ScoreboardEntry : MonoBehaviour
{
	[SerializeField] private Text m_label = null;
	[SerializeField] private Text m_score = null;
	public Player Player => m_player;
	public int Score => m_player.GetScore();

	private Player m_player;
	private Image image;

	private void Awake() {
		image = GetComponent<Image>();
	}

	//store player for this entry
	//set init value and color
	public void Set(Player player)
	{
		m_player = player;
		UpdateScore();
		image.enabled = PhotonNetwork.LocalPlayer == m_player ? true: false;
	}

	//update label bases on score and name
	public void UpdateScore()
	{
		m_label.text = m_player.NickName;
		m_score.text = m_player.GetScore().ToString();
	}
}