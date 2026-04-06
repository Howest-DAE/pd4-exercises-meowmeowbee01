using PD4.MVPBase.Presenter;
using PD4.ShooterGame.Model;
using System;
using UnityEngine;

namespace PD4.ShooterGame.Presenter
{
	public class WeaponSpawnerPresenter : PresenterMonoBehaviour<WeaponSpawnerModel>
	{
		[SerializeField] private GameObject _weaponPrefab;
		[SerializeField] private Transform _spawnLocation;

		[SerializeField] private Material _inactiveGlowMaterial, _inactiveBaseMaterial;
		[SerializeField] private Renderer _glowRenderer, _basePlateRenderer;

		private Material _defaultGlowMaterial, _defaultBaseMaterial;
		private void Awake()
		{
			_defaultGlowMaterial = _glowRenderer.sharedMaterial;
			_defaultBaseMaterial = _basePlateRenderer.sharedMaterial;
			Model = new WeaponSpawnerModel();
		}
		private void Start()
		{
			FindAnyObjectByType<GamePresenter>().Model.AddWeaponSpawner(Model);
		}
		private void Update()
		{
			Model?.UpdateTimer(Time.deltaTime);
		}

		public override void OnModelPropertyChanged(string propertyName)
		{
			if (propertyName == nameof(Model.CanSpawn))
			{
				_glowRenderer.sharedMaterial = Model.CanSpawn ? _defaultGlowMaterial : _inactiveGlowMaterial;
				_basePlateRenderer.sharedMaterial = Model.CanSpawn ? _defaultBaseMaterial : _inactiveBaseMaterial;	
			}
		}

		public void SpawnWeapon()
		{
			if (!Model.CanSpawn) return;
			Model.StartCooldown();

			GameObject instance = Instantiate(_weaponPrefab, _spawnLocation.position, _spawnLocation.rotation);
			WeaponPresenter weaponPresenter = instance.GetComponent<WeaponPresenter>();

		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Player"))
			{
				PlayerModel playerModel = other.GetComponent<PlayerPresenter>()?.Model;
				SpawnWeapon();
			}
		}
	}
}