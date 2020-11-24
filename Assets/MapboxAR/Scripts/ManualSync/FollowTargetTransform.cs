namespace Mapbox.Examples
{
	using UnityEngine;

	public class FollowTargetTransform : MonoBehaviour
	{
		[SerializeField]
		Transform _targetTransform;

		private Singleton singleton;

		private void Start() {
			singleton = Singleton.Instance();
		}

		void Update()
		{
			if(!singleton.isMapMode())
			transform.position = new Vector3(_targetTransform.position.x, transform.position.y, _targetTransform.position.z);
		}
	}
}
