extends Button

func _ready():
    set_process(true)

func _process(delta):
   if Input.is_action_pressed("exit"):
      get_tree().quit()

func _on_quit_button_pressed():
	get_tree().quit()

func _on_start_button_pressed():
	get_tree().change_scene("res://world/world.tscn")