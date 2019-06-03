extends Node

onready var playerScene : Resource = load(utility.construct_scene_path('player'))

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

func create_player() -> void:
	var player : PlayerNode = null

	player = playerScene.instance()
	add_child(player)
	player_globals.player.instance = player
	player.position.x = 250
	player.position.y = 100
	player.initialize_player()
	player.set_camera_zoom(ZOOM)

func enter_crypt() -> void:
	crypt_globals.cryptSeed = -1
	get_tree().change_scene(utility.construct_scene_path('crypt'))

func quit_to_main_menu() -> void:
	get_tree().change_scene(utility.construct_scene_path('main_menu'))