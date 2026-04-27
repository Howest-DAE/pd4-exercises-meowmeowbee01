using PD4.MVPBase.Model;

namespace PD4.ShooterGame.Model
{
	public class WeaponSpawnerModel : ModelBase
	{

		const float _spawnCooldownTime = 3f;
		private float _spawnCooldownTimer = 0f;


		private bool _canSpawn = true;
		public bool CanSpawn
		{
			get => _canSpawn;
			set
			{
				if (_canSpawn == value)
					return;
				_canSpawn = value;
				OnPropertyChanged();
			}
		}

		public void StartCooldown()
		{
			_spawnCooldownTimer = _spawnCooldownTime;
			CanSpawn = false; //deactivate spawner until cooldown is over
		}

		public void UpdateTimer(float deltaTime)
		{
			if (!CanSpawn)
			{
				_spawnCooldownTimer -= deltaTime;
				if (_spawnCooldownTimer <= 0f)
				{
					CanSpawn = true; //reactivate spawner when cooldown is over
				}
			}

		}


	}
}