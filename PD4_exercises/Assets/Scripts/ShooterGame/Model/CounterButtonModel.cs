using PD4.MVPBase.Model;

namespace PD4.ShooterGame.Model
{
	public class CounterButtonModel : ModelBase
	{
		private bool _isPressed;

		public bool IsPressed
		{
			get => _isPressed;
			set
			{
				if (_isPressed == value)
					return;
				_isPressed = value;
				if (_isPressed) //when IsPressed is set to "True", increase the counter
				{
					++Counter;
				}
				OnPropertyChanged();
			}
		}

		private int _counter;

		public int Counter
		{
			get => _counter;
			set
			{
				if (_counter == value)
					return;
				_counter = value;
				OnPropertyChanged();
			}
		}

	}
}