using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class Movement : MonoBehaviour {
	public GameObject m_GameOver;
	public GameObject m_GameManager;
	public Rigidbody m_Bird;
	public GameObject m_Internet;
	obstacle[] all;
	Animation BirdMov;
	public Text m_Tap;
	public Text m_Score;
	public Text Message;
	public Text GameOver_Score;
	public bool m_dead;
	public bool start;
	public int score;
	public float m_speed =200;
	string message;

	// Use this for initialization
	void Start () {
		//Position the Bird when Game starts 
		m_Bird.transform.position = new Vector3 (6, 8.5f, 0);

		//Disable Gameover UI and enable Tap to Start
		m_GameOver.SetActive(false);
		m_Tap.enabled = true;

		//Not Dead and Game not Started
		m_dead = false;
		score = 0;
		start = false;

		all = GameObject.FindObjectsOfType<obstacle> ();
		m_Bird.isKinematic = true;

		//Give Animation
		BirdMov = GetComponent<Animation> ();
		BirdMov ["Take 001"].speed = 0.1f;
		BirdMov.Play ();

		if (PlayerPrefs.GetInt ("Written") == 0) {
			Social.ReportScore (PlayerPrefs.GetInt ("HighScore"), "CgkI-_q7l8QYEAIQAA", (bool success) => {
				if (success)
					PlayerPrefs.SetInt ("Written", 1);
				else
					PlayerPrefs.SetInt ("Written", 0);
			});
		}

	}


	
	// Update is called once per frame
	void Update () {
		if(start && !m_dead)
			m_Tap.enabled = false;		
		if (!m_dead) {
			//If not dead Get Player Input and Give force to the Bird
			if (Input.GetMouseButtonDown (0) && !m_GameManager.GetComponent<Manage> ().quit ) {
				BirdMov ["Take 001"].speed = 1;
				BirdMov.Play ();
				start = true;
				m_Bird.isKinematic = false;
				m_Bird.velocity = Vector3.zero;
				m_Bird.AddForce (transform.up * m_speed);

			}
		} else {
			//Bird Dead
			BirdMov.Stop ();

			//Disable movement of obstacles
			for (int i = 0; i < all.Length; i++) {
				all [i].enabled = false;
			}
			m_Score.enabled = false;

			//Check and Set Highscore
			if (PlayerPrefs.GetInt ("HighScore") < score) {
				Debug.Log ("High");
				PlayerPrefs.SetInt ("HighScore", score);
					Social.ReportScore(PlayerPrefs.GetInt ("HighScore"), "CgkI-_q7l8QYEAIQAA", (bool success) => {
					if(success)
						PlayerPrefs.SetInt("Written",1);
					else
						PlayerPrefs.SetInt("Written",0);
				});
			}

			//GameOver UI
			if (m_GameManager.GetComponent<Manage> ().tracked && !m_GameManager.GetComponent<Manage> ().quit) {
				message = "GAME OVER\n\n<size=27>SCORE</size>\n";
				Message.text = message;
				GameOver_Score.text = "" + score;
				m_GameOver.SetActive (true);
				m_Score.enabled = false;
			} else {
				m_GameOver.SetActive (false);
			}
		}


	}

	//Function for Retry Button
	public void Retry()
	{
		//m_Internet.SetActive (false);
		//Reset Position of Obstacles and Rest UI
		for (int i = 0; i < all.Length; i++) {
			all [i].Restart();
			all [i].enabled = true;
		}
		m_Tap.enabled = true;	

		//Reset Bird Position, Animation, score and UI
		m_Bird.transform.position = new Vector3 (6, 8.5f, 0);
		m_Bird.isKinematic = true;
		score = 0;
		BirdMov ["Take 001"].speed = 0.1f;
		BirdMov.Play ();
		message = "" + score;
		m_Score.text = message;
		m_Score.enabled = true;
		start = false;
		m_dead = false;
		m_GameOver.SetActive(false);
	}
		
	//Function to Increment Score
	void OnTriggerExit()
	{
		if (!m_dead && start) {
			score++;
			message = "" + score;
			m_Score.text = message;
		}
	}

	//Function to Check if the Bird Collided
	void OnCollisionEnter()
	{	
		CapsuleCollider[] all = GameObject.FindObjectsOfType<CapsuleCollider>();
		for (int i = 0; i < all.Length; i++) {
			all [i].isTrigger = true;
		}
		//Make the bird fall downwards
		m_Bird.velocity = new Vector3 (0, -1,0);

		//Bird Dead
		m_dead = true;
		start = false;
	}

}
