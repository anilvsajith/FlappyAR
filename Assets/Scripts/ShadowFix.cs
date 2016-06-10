	using UnityEngine;
	using System.Collections;
	using Vuforia;

	public class ShadowFix : MonoBehaviour, IVideoBackgroundEventHandler
	{
		private Camera[] mCameras;

		// Use this for initialization
		void Start()
		{
			mCameras = VuforiaBehaviour.Instance.GetComponentsInChildren<Camera>();
			VuforiaBehaviour.Instance.RegisterVideoBgEventHandler(this);
		}

		// Update is called once per frame
		void Update () {

		}

		public void OnVideoBackgroundConfigChanged()
		{
			foreach (var cam in mCameras)
			{
				var projMatrix = cam.projectionMatrix;
				for (int i = 0; i < 16; i++)
				{
					if (System.Math.Abs(projMatrix[i]) < 1e-6)
					{
						projMatrix[i] = 0.0f;
					}
				}
				cam.projectionMatrix = projMatrix;
			}
		}
	}
