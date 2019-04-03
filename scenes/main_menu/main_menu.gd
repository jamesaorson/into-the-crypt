extends Control

###################
# Godot Functions #
###################

func _ready() -> void:
	$Menu/StartInCryptButton.visible = OS.is_debug_build()

####################
# Helper Functions #
####################

func load_game() -> void:
	get_tree().change_scene(utility.construct_scene_path('load_game_menu'))

func new_game() -> void:
	get_tree().change_scene(utility.construct_scene_path('village'))

func settings_menu() -> void:
	get_tree().change_scene(utility.construct_scene_path('settings_menu'))

func quit_game() -> void:
	get_tree().quit()

func start_game_in_crypt() -> void:
	var levelSeedSpinBox : SpinBox = $StartInCryptButton/CryptSeed
	if levelSeedSpinBox.value >= 0:
		crypt_globals.cryptSeed = int(levelSeedSpinBox.value)
	else:
		crypt_globals.cryptSeed = -1
	get_tree().change_scene(utility.construct_scene_path('crypt'))

###################
# Signal handlers #
###################

func _on_NewGameButton_pressed():
	new_game()

func _on_QuitButton_pressed():
	quit_game()

func _on_SettingsButton_pressed():
	settings_menu()

func _on_StartInCryptButton_pressed():
	start_game_in_crypt()

func _on_LoadGameButton_pressed():
	load_game()
