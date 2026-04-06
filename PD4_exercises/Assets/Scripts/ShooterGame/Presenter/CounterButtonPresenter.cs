using PD4.MVPBase.Presenter;
using PD4.ShooterGame.Model;
using PD4.ShooterGame.Network;
using PD4.ShooterGame.View;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace PD4.ShooterGame.Presenter
{
	[RequireComponent(typeof(CounterButtonSync))]
	public class CounterButtonPresenter : PresenterMonoBehaviour<CounterButtonModel>
	{
		[Header("Counter")]
		[SerializeField]
		private CounterUIView _view;

		[Header("Button position")]
		[SerializeField]
		private Transform _buttonTransform;
		private Vector3 _defaultButtonPos;

		[SerializeField]
		private float _buttonDownHeight = -0.5f;
		[SerializeField]
		private float _buttonMoveSpeed = 5f;

		private CounterButtonSync _sync;

		private void Awake()
		{
			Model = new CounterButtonModel();
			_sync = GetComponent<CounterButtonSync>();
			_sync.Model = Model;
		}

		void Start()
		{
			_defaultButtonPos = _buttonTransform.localPosition;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Player") && other.GetComponent<NetworkObject>().IsOwner)
			{
				_sync.SetPressedRpc(true);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.CompareTag("Player") && other.GetComponent<NetworkObject>().IsOwner)
			{
				_sync.SetPressedRpc(false);
			}
		}

		public override void OnModelPropertyChanged(string propertyName)
		{
			if (propertyName == nameof(Model.Counter))
			{
				_view.SetValue(Model.Counter);
			}
			else if (propertyName == nameof(Model.IsPressed))
			{
				AnimateButtonPressed();
			}
		}

		private void AnimateButtonPressed()
		{
			StopAllCoroutines();

			Vector3 targetPos = Model.IsPressed ? _defaultButtonPos + Vector3.down * _buttonDownHeight : _defaultButtonPos;
			StartCoroutine(AnimateButtonPos(targetPos));
		}

		IEnumerator AnimateButtonPos(Vector3 targetPos)
		{
			Vector3 startPos = _buttonTransform.localPosition;

			float t = 0f;
			while (t < 1f)
			{
				t += Time.deltaTime * _buttonMoveSpeed;
				t = Mathf.Clamp01(t);

				_buttonTransform.localPosition = Vector3.Lerp(startPos, targetPos, t);

				yield return null; //wait for next frame
			}
		}
	}
}
