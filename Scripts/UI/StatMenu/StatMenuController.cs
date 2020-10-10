using Godot;
using IntoTheCrypt.Models;

namespace IntoTheCrypt.UI.StatMenu
{
	public class StatMenuController : Control
	{
		#region Public

		#region Members
		public Container MenuContainer { get; private set; }
		public Label HealthText { get; private set; }
		public Label StrengthText { get; private set; }
		public Label DexterityText { get; private set; }
		#endregion

		#region Member Methods
		public override void _Ready()
		{
			MenuContainer = GetNode<Container>("MenuContainer");
			HealthText = MenuContainer.GetNode<Label>("GridContainer/HealthLabel");
			StrengthText = MenuContainer.GetNode<Label>("GridContainer/StrengthLabel");
			DexterityText = MenuContainer.GetNode<Label>("GridContainer/DexterityLabel");
			
			SetActive(false);
		}

		public void SetActive(bool isActive)
		{
			MenuContainer.Visible = isActive;
			MenuContainer.SetProcess(isActive);
		}

		public void ToggleActive()
		{
			var isActive = !MenuContainer.IsProcessing();
			SetActive(isActive);
		}

		public void UpdateStats(Stats stats)
		{
			HealthText.Text = $"{stats.HP}/{stats.MaxHP}";
			StrengthText.Text = $"{stats.Strength}";
			DexterityText.Text = $"{stats.Dexterity}";
		}
		#endregion

		#endregion
	}
}
