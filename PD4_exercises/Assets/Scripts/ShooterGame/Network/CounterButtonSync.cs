using PD4.ShooterGame.Model;
using Unity.Netcode;

namespace PD4.ShooterGame.Network
{
	public class CounterButtonSync : NetworkBehaviour
	{
		public CounterButtonModel Model { get; set; }
		public NetworkVariable<bool> IsPressed { get; set; }
		public NetworkVariable<int> Counter { get; set; }

		private void Awake()
		{
			IsPressed = new();
			Counter = new();

			IsPressed.OnValueChanged += (old, value) => Model.IsPressed = value;
			Counter.OnValueChanged += (old, value) => Model.Counter = value;
		}

		public override void OnNetworkSpawn()
		{
			Model.IsPressed = IsPressed.Value;
			Model.Counter = Counter.Value;

			Model.PropertyChanged += OnCounterChanged;
		}

		private void OnCounterChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (!IsServer) return;
			var model = (CounterButtonModel)sender;
			switch (e.PropertyName)
			{
				case nameof(Model.IsPressed):
					IsPressed.Value = model.IsPressed;
					break;
				case nameof(Model.Counter):
					Counter.Value = model.Counter;
					break;
				default:
					break;
			}
		}

		[Rpc(SendTo.Everyone)]
		public void SetPressedRpc(bool pressed)
		{
			Model.IsPressed = pressed;
		}
	}
}