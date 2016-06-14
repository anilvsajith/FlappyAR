﻿using UnityEngine;
using System.Collections;

public class obstacle : MonoBehaviour {
	Transform m_Bottom;
	Transform m_Top;
	public Vector3 ResetPosition;
	public Vector3 RestartPosition;
	public Manage GameManager;
	public float m_limit;
	public float m_Speed,u_speed=2f,limit=1f;
	float m_randpos;
	float m_randsize;
	float pos;
	bool start= false,isUpward=true;
	Vector3 tempTop;

	// Use this for initialization
	void Start () {

		//Get Bottom and Top Obstacles
		m_Bottom = transform.FindChild ("Bottom");
		m_Top = transform.FindChild ("Top");
		pos = m_Bottom.transform.position.y;

		//Give Random Position to the obstacles
		m_randpos = pos + 1 + Random.Range (-1.5f, 1.5f);
		m_randsize = m_randpos + 20.75f+ Random.Range (-0.75f, 0.75f);
		m_Bottom.transform.position = new Vector3(transform.position.x,m_randpos,m_Bottom.transform.position.z);

		tempTop.x = transform.position.x;
		tempTop.z = m_Bottom.transform.position.z;
		tempTop.y = m_randsize;
		m_Top.transform.position = tempTop;
			//new Vector3 (transform.position.x, m_randsize, m_Bottom.transform.position.z);
	}

	//Function to Reset the obstacles on Restart 
	public void Restart()
	{
		start = false;
		m_Top.gameObject.SetActive (false);
		m_Bottom.gameObject.SetActive (false);
		transform.position = RestartPosition;
	}

	// Update is called once per frame
	void Update () {
		
		if (transform.position.x <= 28) {
			m_Top.gameObject.SetActive (true);
			m_Bottom.gameObject.SetActive (true);
		}

		//Start the Obstacle Movement on First Click
		if (Input.GetMouseButtonDown (0) && start == false && GameManager.paused == false ) {
			start = true;		

		}
		//Movement of Obstacle
		if(start){

			if (transform.position.x > m_limit) {
				transform.Translate (Vector3.left * Time.deltaTime* m_Speed);
			} else {
				//Reset Position and give random size on reaching limit
				m_randpos = pos + 1 + Random.Range (-1.5f, 1.5f);
				m_randsize = m_randpos + 20.75f+ Random.Range (-0.75f, 0.75f);
				m_Bottom.transform.position = new Vector3(transform.position.x,m_randpos,m_Bottom.transform.position.z);

				tempTop.x = transform.position.x;
				tempTop.z = m_Bottom.transform.position.z;
				tempTop.y = m_randsize;
				m_Top.transform.position = tempTop;

				m_Top.gameObject.SetActive (false);
				m_Bottom.gameObject.SetActive (false);
				transform.position = ResetPosition;
			}

		}

		if (GameManager.paused == false) {
			Debug.Log ("Called");
			if (isUpward) {
				Debug.Log (tempTop.y + "  " + (m_randsize + limit));
				if (tempTop.y < m_randsize + limit)
					tempTop.y += u_speed * Time.deltaTime;
				else
					isUpward = false;	
			} else {
				if (tempTop.y > m_randsize)
					tempTop.y -= u_speed * Time.deltaTime;
				else
					isUpward = true;
			}
			tempTop.x = m_Bottom.transform.position.x;
			m_Top.transform.position = tempTop;

		}
	
	}


}
