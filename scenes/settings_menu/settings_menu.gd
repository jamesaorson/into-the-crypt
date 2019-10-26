extends Control

###################
# Godot Functions #
###################

func _ready() -> void:
	$Menu/ReturnButton.grab_focus()

####################
# Helper Functions #
####################

func main_menu() -> void:
	utility.error_handled_scene_change('main_menu')

###################
# Signal handlers #
###################

func _on_ReturnButton_pressed():
	main_menu()