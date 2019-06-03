extends Control

###################
# Godot Functions #
###################

####################
# Helper Functions #
####################

func main_menu() -> void:
	get_tree().change_scene(utility.construct_scene_path('main_menu'))

###################
# Signal handlers #
###################

func _on_ReturnButton_pressed():
	main_menu()