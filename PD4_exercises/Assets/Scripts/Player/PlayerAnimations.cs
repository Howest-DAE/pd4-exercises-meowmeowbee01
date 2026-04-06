using PD4.MVPBase.Presenter;
using PD4.ShooterGame.Model;
using PD4.ShooterGame.Presenter;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerAnimations : PresenterMonoBehaviour<PlayerModel>
{
	[SerializeField]
	private float _moveScale = 2f;

	const string _moveXParameter = "MoveX";
	const string _moveYParameter = "MoveY";
	const string _crouchParameter = "Crouch";
	const string _grabParameter = "Aim";


	private Vector3 _prevPosition;
	private Vector2 _animDampVel;
	private Vector2 _moveAnimVel;
	[SerializeField]
	private float _smoothTime = 0.5f;


	[SerializeField]
	private Animator _animator;

	private void Awake()
	{
		//_animator = GetComponent<Animator>();
	}

	private void Start()
	{
		_prevPosition = transform.position;
		var presenter = GetComponentInParent<PlayerPresenter>();
		this.Model = presenter.Model;
	}
	void LateUpdate()
	{
		Vector3 move = transform.InverseTransformVector(transform.position - _prevPosition);
		Vector3 vel = move / Time.deltaTime;
		Vector2 vel2d = new Vector2(vel.x, vel.z);

		_moveAnimVel = Vector2.SmoothDamp(_moveAnimVel, vel2d, ref _animDampVel, _smoothTime);


		_animator.SetFloat(_moveXParameter, _moveAnimVel.x * _moveScale);
		_animator.SetFloat(_moveYParameter, _moveAnimVel.y * _moveScale);
		//_animator.SetBool(_crouchParameter, _move.IsCrouching);

		_prevPosition = transform.position;
	}

	public override void OnModelPropertyChanged(string propertyName)
	{
		if (propertyName == nameof(Model.PickedUpWeapon))
		{
			_animator.SetBool(_grabParameter, Model.PickedUpWeapon != null);
		}
	}
}
