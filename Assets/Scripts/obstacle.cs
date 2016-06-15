using UnityEngine;
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
	float spacing;
	float pos;
	bool start= false,isUpward=true,activated=false,topMoving;
	Vector3 tempTop;
	float minGap;

	// Use this for initialization
	void Start () {


		topMoving = false;
		
		activated = false;
		minGap = 97f;
		//Get Bottom and Top Obstacles
		m_Bottom = transform.FindChild ("Bottom");
		m_Top = transform.FindChild ("Top");
		pos = transform.localPosition.y;

	}

	//Function to Reset the obstacles on Restart 
	public void Restart()
	{
		start = false;
		m_Top.gameObject.SetActive (false);
		m_Bottom.gameObject.SetActive (false);
		transform.localPosition = RestartPosition;
		setRandPos ();
		// top won't move for the first 4 obstacles
		topMoving = false;
		activated = false;
	}

	void setRandPos (){
		float a = Random.Range (0f, 5f);
		if (a > 4.3f||a<0.4f)
			topMoving = true;
		else
			topMoving = false;
		//Give Random Position to the obstacles
		m_randpos = pos + Random.Range (-1f, 3.5f);
		spacing = minGap + Random.Range (0, 10f);

		Debug.Log (spacing);

		transform.localPosition = new Vector3(transform.localPosition.x,m_randpos,transform.localPosition.z);

		tempTop.x = 0;
		tempTop.z = 0;
		tempTop.y = spacing;
		m_Top.transform.localPosition = tempTop;
	}

	// Update is called once per frame
	void Update () {
		
		if (transform.localPosition.x <= 28&&activated == false) {
			m_Top.gameObject.SetActive (true);
			m_Bottom.gameObject.SetActive (true);
			activated = true;
		}

		//Start the Obstacle Movement on First Click
		if (Input.GetMouseButtonDown (0) && start == false && GameManager.paused == false ) {
			start = true;		

		}
		//Movement of Obstacle
		if(start){

			if (transform.localPosition.x > m_limit) {
				transform.Translate (Vector3.left * Time.deltaTime* m_Speed);
			} else {
				//Reset Position and give random size on reaching limit
				transform.localPosition = ResetPosition;
				setRandPos();
			}

		}

		if (GameManager.paused == false&&topMoving) {
			
			if (isUpward) {
				if (tempTop.y < spacing + limit)
					tempTop.y += u_speed * Time.deltaTime;
				else
					isUpward = false;	
			} else {
				tempTop.y -= u_speed * Time.deltaTime;
				if (tempTop.y > spacing)
					;
				else {
					isUpward = true;
					tempTop.y = spacing;
				}
			}

			m_Top.transform.localPosition = tempTop;

		}
	
	}


}
