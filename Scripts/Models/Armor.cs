using Godot;

namespace IntoTheCrypt.Models
{
	public struct Armor
	{
		#region Public

		#region Constructors
		public Armor(uint maxRating)
		{
			_maxRating = maxRating;
			_rating = maxRating;
		}
		#endregion

		#region Members
		public uint MaxRating
		{
			get => _maxRating;
			set
			{
				_maxRating = value;
				if (value < _rating)
				{
					Rating = MaxRating;
				}
			}
		}
		
		public uint Rating
		{
			get => _rating;
			set
			{
				if (value <= _maxRating)
				{
					_rating = value;
				}
				else
				{
					_rating = _maxRating;
				}
			}
		}
		#endregion

		#endregion

		#region Private

		#region Members
		private uint _maxRating;
		private uint _rating;
		#endregion

		#endregion
	}
}
