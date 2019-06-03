extends Control

###################
# Godot Functions #
###################

func _ready() -> void:
	$StartInCryptButton.visible = OS.is_debug_build()

####################
# Helper Functions #
####################

func quit_game() -> void:
	get_tree().quit()

func start_game() -> void:
	get_tree().change_scene("res://village/village.tscn")

func start_game_in_crypt() -> void:
	var levelSeedSpinBox : SpinBox = $StartInCryptButton/CryptSeed
	if levelSeedSpinBox.value >= 0:
		crypt_globals.cryptSeed = int(levelSeedSpinBox.value)
	else:
		crypt_globals.cryptSeed = -1
	get_tree().change_scene("res://crypt/crypt.tscn")

###################
# Signal handlers #
###################

func _on_QuitButton_pressed():
	quit_game()

func _on_StartButton_pressed():
	start_game()

func _on_StartInCryptButton_pressed():
	start_game_in_crypt()