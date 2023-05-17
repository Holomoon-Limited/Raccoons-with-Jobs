using UnityEngine;
using System.Collections.Generic;
using Cinemachine;

// fix to work with cinnemachine 
public class CameraShake : MonoBehaviour
{
	public static CameraShake instance;
	[SerializeField] private CinemachineBrain cineBrain;

	[SerializeField] private List<Transform> allCamTransforms = new List<Transform>();
	// Transform of the camera to shake. Grabs the gameObject's transform
	[SerializeField] Transform camToShakeTransform;

	// How long the object should shake for.
	public float shakeDuration = 0f;

	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.3f;
	public float decreaseFactor = 1.0f;

	Vector3 originalPos;

	// keeps list of virtual cameras in scene 
	private void OnValidate()
	{
		cineBrain = gameObject.GetComponent<CinemachineBrain>();
		
		CinemachineVirtualCamera[] allCams = FindObjectsOfType<CinemachineVirtualCamera>();
		// if don't have all cam transforms or list is null then populate it 
		if (allCams.Length != allCamTransforms.Count || allCamTransforms == null)
		{
			for (int i = 0; i < allCams.Length; i++)
			{
				Transform t = allCams[i].GetComponent<Transform>();
				allCamTransforms.Add(t);
			}
		}
	}

	void Awake()
	{
		CameraShake.instance = this;
		
		if (camToShakeTransform == null)
		{
			camToShakeTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}

	void OnEnable()
	{
		originalPos = camToShakeTransform.localPosition;
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
			camToShakeTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

			shakeDuration -= Time.deltaTime * decreaseFactor;
		}
		else
		{
			shakeDuration = 0f;
			camToShakeTransform.localPosition = originalPos;
		}
	}

	private void OnGUI()
	{

		if (GUILayout.Button("Test shake"))
		{
			CameraShakeByTime(1f,1f);
		}
	}
}