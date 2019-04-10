extends Button

var cryptScene = preload("res://crypt/crypt.tscn")

func _ready():
	set_process(true)

func _process(delta):
	if Input.is_action_pressed("ui_accept"):
		start_game()
	if Input.is_action_pressed("ui_cancel") or Input.is_action_pressed("exit"):
		quit_game()

func _on_quit_button_pressed():
	quit_game()

func _on_start_button_pressed():
	start_game()

func quit_game():
	get_tree().quit()

func start_game():
	get_tree().change_scene_to(cryptScene)