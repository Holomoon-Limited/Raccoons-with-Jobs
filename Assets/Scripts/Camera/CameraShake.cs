using UnityEngine;

namespace Holo.Cam
{
	public class CameraShake : MonoBehaviour
	{
		public static CameraShake instance;

		[SerializeField] private Transform[] allCamTransforms = new Transform[] { };

		// How long the object should shake for.
		[SerializeField] private float shakeTime;
		private float shakeTimer;

		// Amplitude of the shake. A larger value shakes the camera harder.
		[SerializeField] private float shakeAmount;
		
		// how fast timer goes down. higher value creates a shorter burst
		[SerializeField] private float decreaseFactor;

		private Vector3 vCam0OriginalPos;
		private Vector3 vCam1OriginalPos;

		void Awake()
		{
			CameraShake.instance = this;
		}

		void OnEnable()
		{
			vCam0OriginalPos = allCamTransforms[0].localPosition;
			vCam1OriginalPos = allCamTransforms[1].localPosition;
		}

		public void StandardCameraShake()
		{
			// sets timer by shakeTime 
			shakeTimer = shakeTime;
		}

		public void CustomCameraShake(float shaketime, float shakeIntensity)
		{
			shakeTimer = shaketime;
			shakeAmount = shakeIntensity;
		}

		void Update()
		{
			if (shakeTimer > 0)
			{
				allCamTransforms[0].localPosition = vCam0OriginalPos + Random.insideUnitSphere * shakeAmount;
				allCamTransforms[1].localPosition = vCam1OriginalPos + Random.insideUnitSphere * shakeAmount;

				shakeTimer -= Time.deltaTime * decreaseFactor;
			}
			else
			{
				shakeTimer = 0f;
				allCamTransforms[0].localPosition = vCam0OriginalPos;
				allCamTransforms[1].localPosition = vCam1OriginalPos;
			}
		}

		private void OnGUI()
		{
			if (GUILayout.Button("Test shake"))
			{
				StandardCameraShake();
			}
		}
	}
}