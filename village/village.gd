extends Node

onready var playerScene = load("res://player/player.tscn")

export(Vector2) var ZOOM = Vector2(0.4, 0.4)

###################
# Godot Functions #
###################

func _ready():
	create_player(0)

func _process(delta):
	if Input.is_action_pressed(input_globals.PAUSE):
		quit_to_main_menu()

####################
# Helper Functions #
####################

func cleanup():
	var villageNodes = get_tree().get_nodes_in_group("village")
	for node in villageNodes:
		node.destroy()

func create_player(playerIndex):
	if playerIndex != null:
		var player = null
		if player_globals.players[playerIndex].instance != null:
			player = player_globals.players[playerIndex].instance
		else:
			player = playerScene.instance()
			player_globals.players[playerIndex].instance = player
			player.set_player_index(playerIndex)
			add_child(player)
		player.position.x = 250
		player.position.y = 100
		player.set_player_index(playerIndex)
		player.set_camera_zoom(ZOOM)
		player_globals.players[playerIndex].timeStart = OS.get_unix_time()

func destroy():
	var playerNodes = get_tree().get_nodes_in_group("player")
	for node in playerNodes:
		node.destroy()
	for player in player_globals.players:
		player.instance = null
		player.debugInfo = null
		player.lightNode = null

	queue_free()

func enter_crypt():
	cleanup()
	crypt_globals.cryptSeed = null
	get_tree().change_scene("res://crypt/crypt.tscn")

func quit_to_main_menu():
	cleanup()
	get_tree().change_scene("res://ui/main_menu/main_menu.tscn")