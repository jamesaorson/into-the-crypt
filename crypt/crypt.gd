extends Node

onready var roomScene = load("res://crypt/room/room.tscn")

func _ready():
	set_process(true)
	create_room()

func _process(delta):
	if Input.is_action_pressed("pause"):
		quit_game()

func create_room():
	print("creating room...")
	var rooms = get_tree().get_nodes_in_group("room")
	if len(rooms) == 0:
		var room = roomScene.instance()
		get_tree().root.add_child(room)

func quit_game():
	get_tree().quit()