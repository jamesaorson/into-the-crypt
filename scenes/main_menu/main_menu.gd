extends Control

###################
# Godot Functions #
###################

func _ready() -> void:
	$Menu/StartInCryptButton.visible = OS.is_debug_build()
	$Menu/NewGameButton.grab_focus()

####################
# Helper Functions #
####################

func load_game() -> void:
	utility.error_handled_scene_change('load_game_menu')

func new_game() -> void:
	utility.error_handled_scene_change('village')

func quit_game() -> void:
	get_tree().quit()

func settings_menu() -> void:
	utility.error_handled_scene_change('settings_menu')

func start_game_in_crypt() -> void:
	var levelSeedSpinBox : SpinBox = $Menu/StartInCryptButton/CryptSeed
	if levelSeedSpinBox.value >= 0:
		crypt_manager_globals.cryptSeed = int(levelSeedSpinBox.value)
	else:
		crypt_manager_globals.cryptSeed = -1
	utility.error_handled_scene_change('crypt_manager')

###################
# Signal handlers #
###################

func _on_LoadGameButton_pressed():
	load_game()

func _on_NewGameButton_pressed():
	new_game()

func _on_QuitButton_pressed():
	quit_game()

func _on_SettingsButton_pressed():
	settings_menu()

func _on_StartInCryptButton_pressed():
	start_game_in_crypt()
