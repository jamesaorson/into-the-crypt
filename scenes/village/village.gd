extends Node

onready var playerScene : Resource = load(utility.construct_scene_path('player'))

export(Vector2) var ZOOM : Vector2 = Vector2(0.4, 0.4)

###################
# Godot Functions #
###################

func _ready() -> void:
	create_player()

####################
# Helper Functions #
####################

func create_player() -> void:
	var player : PlayerNode = null

	player = playerScene.instance()
	add_child(player)
	player.player.instance = player
	player.position.x = 250
	player.position.y = 100
	player.initialize_player()
	player.set_camera_zoom(ZOOM)

func enter_crypt() -> void:
	crypt_manager_globals.cryptSeed = -1
	utility.error_handled_scene_change('crypt_manager')
