using PD4.MVPBase.Model;

namespace PD4.MVPBase.Presenter
{
	public abstract class PresenterBase<T> : IPresenter<T> where T : ModelBase
	{
		private T _model;
		public T Model
		{
			get => _model;
			set
			{
				if (_model == null && value == null) return;
				if (_model != null && _model.Equals(value)) return;

				T prevModel = _model;
				if (_model != null)
				{
					_model.PropertyChanged -= _model_PropertyChanged;
				}
				_model = value;

				if (_model != null)
				{
					_model.PropertyChanged += _model_PropertyChanged;
				}

				OnModelUpdated(prevModel);
			}
		}

		public abstract void OnModelPropertyChanged(string propertyName);

		public virtual void OnModelUpdated(T previousModel)
		{

		}

		private void _model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			OnModelPropertyChanged(e.PropertyName);
		}
	}
}
