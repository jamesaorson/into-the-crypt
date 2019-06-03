extends Node

onready var playerScene : Resource = load("res://player/player.tscn")

export(Vector2) var ZOOM : Vector2 = Vector2(0.4, 0.4)

###################
# Godot Functions #
###################

func _ready() -> void:
	create_player()

func _process(delta : float) -> void:
	if Input.is_action_pressed(input_globals.PAUSE):
		quit_to_main_menu()

####################
# Helper Functions #
####################

func cleanup() -> void:
	var villageNodes : Array = get_tree().get_nodes_in_group("village")
	for node in villageNodes:
		node.destroy()

func create_player() -> void:
	var player : PlayerNode = null
	if player_globals.player.instance != null:
		player = player_globals.player.instance
	else:
		player = playerScene.instance()
		player_globals.player.instance = player
		add_child(player)
	player.position.x = 250
	player.position.y = 100
	player.initialize_player()
	player.set_camera_zoom(ZOOM)

func destroy() -> void:
	var playerNodes : Array = get_tree().get_nodes_in_group("player")
	for node in playerNodes:
		node.destroy()
	player_globals.player.instance = null
	player_globals.player.debugInfo = null
	player_globals.player.lightNode = null

	queue_free()

func enter_crypt() -> void:
	cleanup()
	crypt_globals.cryptSeed = -1
	get_tree().change_scene("res://crypt/crypt.tscn")

func quit_to_main_menu() -> void:
	cleanup()
	get_tree().change_scene("res://ui/main_menu/main_menu.tscn")