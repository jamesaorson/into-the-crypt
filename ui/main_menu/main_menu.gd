extends Button

###################
# Signal handlers #
###################

func _on_quit_button_pressed():
	quit_game()

func _on_start_button_pressed():
	start_game()

func _on_start_dev_button_pressed():
	start_game_dev()

####################
# Helper Functions #
####################

func quit_game():
	get_tree().quit()

func start_game_dev():
	var levelSeedSpinBoxNode = get_node("/root/main_menu_control/start_dev_button/level_seed_spin_box")
	if levelSeedSpinBoxNode.value >= 0:
		crypt_globals.cryptSeed = levelSeedSpinBoxNode.value
	else:
		crypt_globals.cryptSeed = null
	get_tree().change_scene("res://crypt/crypt.tscn")

func start_game():
	get_tree().change_scene("res://village/village.tscn")