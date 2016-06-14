﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class Manage : MonoBehaviour {
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
	GameObject Game;
	bool armode;
	public Text m_Score;
	public Renderer m_Floor;

	// Use this for initialization
	void Start () {
		Game = ImageTarget.transform.Find ("Game").gameObject;
		begin = false;
		armode = true;
	}

	//Function to check if Marker Lost
	public void OnTrackingLost()
	{
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

		//UI for Tap to Start, Resume and disable  Marker Lost/TitleScreen
		m_Tap.SetActive (true);
		if (bird.GetComponent<Movement> ().m_dead) {
			m_GameOver.SetActive (true);
		}
		m_Marker.SetActive (false);
		TitleScreen.SetActive (false);
		if (bird.GetComponent<Movement> ().start)
			ResumeButton.SetActive (true);
		else {
			bird.GetComponent<Movement> ().enabled = true;
			m_Score.enabled = true;

			paused = false;
		}
			
	}
		
	//Resume the game
	public void Resume()
	{
		paused = false;
		m_Score.enabled = true;
		Time.timeScale = 1;
		//Enable User Control over the Bird
		bird.GetComponent<Movement>().enabled = true;
		//Disable Resume and Quit UI 
		ResumeButton.SetActive (false);	
		QuitOption.SetActive (false);
	}

	//Function to Quit
	public void Quit()
	{
		Application.Quit ();
	}

	// Update is called once per frame
	void Update () {

		if (bird.GetComponent<Movement>().m_dead || !begin) {
			ArNonAr.SetActive (true);
		} else {
			ArNonAr.SetActive (false);
		}

		//Check if Back Button is Pressed
		if (Input.GetKey (KeyCode.Escape)) {
			//Disable All UI except Quit Message
			m_Tap.SetActive (false);
			m_GameOver.SetActive (false);
			ResumeButton.SetActive (false);
			m_Marker.SetActive (false);
			TitleScreen.SetActive (false);
			m_Score.enabled = false;
			QuitOption.SetActive (true);

			//Pause the game and disable movement
			paused = true;
			bird.GetComponent<Movement>().enabled = false;
			if (bird.GetComponent<Movement> ().start){
				Time.timeScale = 0;
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
		PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkI-_q7l8QYEAIQAA");
	}

	public void ARmode()
	{
		if (armode == true) {
			armode = false;
			ARCam.SetActive (false);
			MainCam.SetActive (true);
			Game.transform.parent = null;
			ImageTarget.SetActive (false);
			Game.GetComponent<Armode> ().TrackingFound ();
			OnTrackingFound ();	
		} else {
			Game.GetComponent<Armode> ().TrackingLost ();
			OnTrackingLost ();
			armode = true;
			ARCam.SetActive (true);
			MainCam.SetActive (false);
			Game.transform.parent = ImageTarget.transform;
			ImageTarget.SetActive (true);
		}
	}
}
