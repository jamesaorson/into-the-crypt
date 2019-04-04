extends Node

onready var roomScene = load("res://crypt/room/room.tscn")

func _ready():
	set_process(true)
	create_room()

func _process(delta):		
	if Input.is_action_pressed("pause"):
		quit_game()

func create_room():
	var room = null
	var roomNodes = get_tree().get_nodes_in_group("room")
	if len(roomNodes) != 0:
		room = roomNodes[0]
	else:
		print("Creating room...")
		room = roomScene.instance()
		get_tree().root.add_child(room)
	room.generate_room()

func quit_game():
	get_tree().quit()