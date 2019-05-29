extends Control

###################
# Godot Functions #
###################

func _ready():
	$StartInCryptButton.visible = OS.is_debug_build()

####################
# Helper Functions #
####################

func quit_game():
	get_tree().quit()

func start_game_in_crypt():
	var levelSeedSpinBoxNode = $StartInCryptButton/CryptSeed
	if levelSeedSpinBoxNode.value >= 0:
		crypt_globals.cryptSeed = levelSeedSpinBoxNode.value
	else:
		crypt_globals.cryptSeed = null
	get_tree().change_scene("res://crypt/crypt.tscn")

func start_game():
	get_tree().change_scene("res://village/village.tscn")

###################
# Signal handlers #
###################

func _on_QuitButton_pressed():
	quit_game()

func _on_StartButton_pressed():
	start_game()

func _on_StartInCryptButton_pressed():
	start_game_in_crypt()