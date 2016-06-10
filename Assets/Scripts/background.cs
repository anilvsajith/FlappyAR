using UnityEngine;
using System.Collections;

public class background : MonoBehaviour {
	public Movement bird;
	//public float maxscale;
	//public float minscale;
	//public float maxpos;
	//public float minpos;
	//float scale;
	float m_speed;
	Vector3 RestartPosition;
	Transform[] bushes;
	// Use this for initialization
	void Start () {
		//Get all bushes
		bushes = GetComponentsInChildren<Transform> ();	
	}
			
	// Update is called once per frame
	void Update () {		
		//Move the bushes if the game is going on
		if (bird.start) {
			m_speed = 2.5f + 1;
		} else {
			m_speed = 2.5f;
		}
		if (!bird.m_dead ) {
			for (int i = 1; i < bushes.Length; i++) {
				bushes [i].Translate (Vector3.left * m_speed * Time.deltaTime);
				if (bushes [i].position.x < -13.35f) {
					//scale = Random.Range (maxscale, minscale);
					//bushes [i].localScale = new Vector3 (scale, scale, scale);
					bushes [i].position = new Vector3 (29.25f, bushes [i].position.y, bushes[i].position.z);
				}
			}
		}
	}
}
