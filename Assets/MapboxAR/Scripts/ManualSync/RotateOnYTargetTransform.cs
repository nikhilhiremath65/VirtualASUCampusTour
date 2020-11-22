namespace Mapbox.Examples
{
	using UnityEngine;

	public class RotateOnYTargetTransform : MonoBehaviour
	{
		[SerializeField]
		Transform _targetTransform;

		[SerializeField]
		Transform ARPlayer;

		void Update()
		{
			transform.position = ARPlayer.position;
			transform.eulerAngles = new Vector3(transform.eulerAngles.x, _targetTransform.eulerAngles.y, transform.eulerAngles.z);
		}
	}
}
