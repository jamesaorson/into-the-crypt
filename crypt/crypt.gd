extends Node

func _ready():
	set_process(true)

func _process(delta):
	if Input.is_action_pressed("pause"):
		quit_game()

func quit_game():
	get_tree().quit()