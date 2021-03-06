﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class Manage : MonoBehaviour {
	public Animator Internet;
	public GameObject m_Inter;
	public GameObject bird; 
	public GameObject ARCam;
	public GameObject MainCam;
	public GameObject ImageTarget;
	public GameObject m_GameOver;
	public GameObject m_Marker;
	public GameObject ResumeButton;
	public GameObject QuitOption;
	public GameObject m_Tap;
	public GameObject TitleScreen;
	public GameObject ArNonAr;
	public bool paused;
	bool begin;
	public bool tracked;
	public bool quit;
	GameObject Game;
	bool armode;
	public Text m_Score;
	public Renderer m_Floor;
	//WaitForSeconds m_delay;
	// Use this for initialization
	void Start () {
		//m_delay = new WaitForSeconds (4.0f);
		Game = ImageTarget.transform.Find ("Game").gameObject;
		begin = false;
		armode = true;
		quit = false;
	}

	//Function to check if Marker Lost
	public void OnTrackingLost()
	{
		tracked = false;
		paused = true;
		bird.GetComponent<Movement>().enabled = false;
		if (bird.GetComponent<Movement> ().start){
			Time.timeScale = 0;
		}

		//Disable UI
		m_Tap.SetActive (false);
		m_GameOver.SetActive (false);
		ResumeButton.SetActive (false);
		m_Marker.SetActive (false);
		TitleScreen.SetActive (false);
		m_Score.enabled = false;

		//Check if TitleScreen or Lost marker message needs to be shown
		if (!begin) {
			TitleScreen.SetActive (true);
		} else {
			m_Marker.SetActive (true);
		}
	}

	//Function to check if Marker Found
	public void OnTrackingFound()
	{
		//Bool to decide TitleScreen or Marker message 
		begin = true;
		tracked = true;
		//UI for Tap to Start, Resume and disable  Marker Lost/TitleScreen
		m_Tap.SetActive (true);
		if (bird.GetComponent<Movement> ().m_dead && !quit) {
			m_GameOver.SetActive (true);
			//m_Score.enabled = false;
		}
		m_Marker.SetActive (false);
		TitleScreen.SetActive (false);
		if (bird.GetComponent<Movement> ().start && !quit)
			ResumeButton.SetActive (true);
		else {
			bird.GetComponent<Movement> ().enabled = true;
			if(!quit && !bird.GetComponent<Movement> ().m_dead)
				m_Score.enabled = true;

			paused = false;
		}
			
	}
		
	//Resume the game
	public void Resume()
	{		
		//m_Internet.SetActive (false);
		paused = false;
		m_Score.enabled = true;
		Time.timeScale = 1;
		//Enable User Control over the Bird
		bird.GetComponent<Movement> ().enabled = true;
		//Disable Resume and Quit UI 
		ResumeButton.SetActive (false);	
	}

	public void NoQuit()
	{
		//m_Internet.SetActive (false);
		quit = false;
		QuitOption.SetActive (false);
		if (!tracked) {
			m_GameOver.SetActive (false);
			m_Marker.SetActive (true);
		}
		if (!bird.GetComponent<Movement> ().m_dead && tracked) {
			paused = false;
			if (bird.GetComponent<Movement> ().start) {
				Time.timeScale = 1;
				m_Score.enabled = true;
			}
		}
	}

	//Function to Quit
	public void Quit()
	{
		Application.Quit ();
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.G))
		{
			OnTrackingLost ();
		}
		if (Input.GetKey (KeyCode.H)) {
			OnTrackingFound ();
		}
		if (((bird.GetComponent<Movement>().m_dead || !begin) && !quit && !paused) || !begin){
			if (armode) {
				ArNonAr.GetComponentInChildren<Text> ().text = "Non AR";
			} else {
				ArNonAr.GetComponentInChildren<Text> ().text = "AR";
			}
			ArNonAr.SetActive (true);
		} else {
			ArNonAr.SetActive (false);
		}

		//Check if Back Button is Pressed
		if (Input.GetKey (KeyCode.Escape)) {
			//Disable All UI except Quit Message
			//m_Internet.SetActive (false);
			quit = true;
			m_Tap.SetActive (false);
			m_GameOver.SetActive (false);
			ResumeButton.SetActive (false);
			m_Marker.SetActive (false);
			TitleScreen.SetActive (false);
			m_Score.enabled = false;
			ArNonAr.SetActive (false);
			QuitOption.SetActive (true);
		
			//Pause the game and disable movement
			if (!bird.GetComponent<Movement> ().m_dead) {
				paused = true;
				if (bird.GetComponent<Movement> ().start) {
					Time.timeScale = 0;
				}
			}


		}
		//Move the Floor
		if (!bird.GetComponent<Movement> ().m_dead) {
			m_Floor.material.SetTextureOffset ("_MainTex", new Vector2 (Time.time * 0.25f, 0));
		} else {
			m_Floor.material.SetTextureOffset ("_MainTex", new Vector2 (0, 0));
		}
	}

	public void ShowLeaderBoard()
	{
		if(PlayGamesPlatform.Instance.IsAuthenticated())
			PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkI-_q7l8QYEAIQAA");
		else
			Social.localUser.Authenticate((bool success)=>{
				if(success)
					PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkI-_q7l8QYEAIQAA");
				else
					NoInternet();
			});

	}

	public void ARmode()
	{
		if (armode == true) {
			armode = false;
			ARCam.SetActive (false);
			MainCam.SetActive (true);
			//m_Internet.SetActive (false);
			Game.transform.parent = null;
			ImageTarget.SetActive (false);
			Game.GetComponent<Armode> ().TrackingFound ();
			OnTrackingFound ();	
		} else {
			Game.GetComponent<Armode> ().TrackingLost ();
			OnTrackingLost ();
			armode = true;
			//m_Internet.SetActive (false);
			ARCam.SetActive (true);
			MainCam.SetActive (false);
			Game.transform.parent = ImageTarget.transform;
			ImageTarget.SetActive (true);
		}
	}

	public void NoInternet()
	{
		Internet.SetTrigger ("NoInternet");
		//Internet.Play ("NoInternet");		
		//StartCoroutine(Internet());
	}
//	private IEnumerator Internet(){
//		m_Internet.SetActive (true);
//		yield return m_delay;
//		m_Internet.SetActive (false);
//	}

}
