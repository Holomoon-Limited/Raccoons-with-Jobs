using UnityEngine;

namespace Holo.Cam
{
	public class CameraShake : MonoBehaviour
	{
		public static CameraShake instance;

		[SerializeField] private Transform[] allCamTransforms = new Transform[] { };

		// How long the object should shake for.
		public float shakeDuration = 0f;

		// Amplitude of the shake. A larger value shakes the camera harder.
		public float shakeAmount = 0.3f;
		public float decreaseFactor = 1.0f;

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

		public void CameraShakeByTime(float shaketime, float shakeIntensity)
		{
			shakeDuration = shaketime;
			shakeAmount = shakeIntensity;
		}

		void Update()
		{
			if (shakeDuration > 0)
			{
				allCamTransforms[0].localPosition = vCam0OriginalPos + Random.insideUnitSphere * shakeAmount;
				allCamTransforms[1].localPosition = vCam1OriginalPos + Random.insideUnitSphere * shakeAmount;

				shakeDuration -= Time.deltaTime * decreaseFactor;
			}
			else
			{
				shakeDuration = 0f;
				allCamTransforms[0].localPosition = vCam0OriginalPos;
				allCamTransforms[1].localPosition = vCam1OriginalPos;
			}
		}

		private void OnGUI()
		{
			if (GUILayout.Button("Test shake"))
			{
				CameraShakeByTime(0.5f, shakeAmount);
			}
		}
	}
}