using System;
using Godot;
using static Godot.Input;

public class MainMenuController : Control
{
	#region Public

	#region Member Methods
	public override void _Ready()
	{
		LoadScenes();
		Input.SetMouseMode(MouseMode.Visible);
	}
	#endregion

	#endregion

	#region Private
	
	#region Members
	private PackedScene _gameScene;
	#endregion

	#region Member Methods
	private void LoadScenes()
	{
		_gameScene = GD.Load<PackedScene>("res://Scenes/Game.tscn");
		if (_gameScene == null)
		{
			throw new Exception("Game scene did not load correctly");
		}
	}
	#endregion

	#region Signals
	private void _on_ExitGameButton_pressed()
	{
		GetTree().Quit();
	}

	private void _on_LoadGameButton_pressed()
	{
		GD.Print("Load game");
	}

	private void _on_NewGameButton_pressed()
	{
		GetTree().ChangeSceneTo(_gameScene);
	}

	private void _on_SettingsButton_pressed()
	{
		GD.Print("Settings");
	}
	#endregion
	
	#endregion
}
