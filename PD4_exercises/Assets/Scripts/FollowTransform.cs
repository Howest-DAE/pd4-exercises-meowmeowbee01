using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class FollowTransform : MonoBehaviour
	{
		private List<Transform> _children = new();
		public void AddChild(Transform child)
		{
			_children.Add(child);
		}
		public void RemoveChild(Transform child)
		{
			_children.Remove(child);
		}
		private void LateUpdate()
		{
			_children.ForEach(c => c.SetLocalPositionAndRotation(transform.position, transform.rotation));
		}
	}
}