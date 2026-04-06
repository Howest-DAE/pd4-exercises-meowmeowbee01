using System;

namespace PD4.ShooterGame.Model
{
	public class PlayerEventArgs: EventArgs
	{
		public PlayerEventArgs(PlayerModel playerModel)
		{
			PlayerModel = playerModel;
		}

		public PlayerModel PlayerModel { get; }
	}
}