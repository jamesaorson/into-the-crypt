extends Node

onready var cryptGenerator = load("res://crypt/crypt_generator/crypt_generator.tscn")

###################
# Godot Functions #
###################

var shouldGenerate = false

func _process(delta):
	if Input.is_action_pressed(input_globals.PAUSE):
		quit_to_main_menu()
	if OS.is_debug_build() and Input.is_action_pressed(input_globals.RESET):
		create_crypt(true)

func _ready():
	set_process(true)
	create_crypt()
	change_music()

####################
# Helper Functions #
####################

func change_music(streamPath = null, volume = -20):
	if streamPath == null:
		streamPath = "res://assets/audio/bgm/passive_crypt_" + str(randi() % 2) + ".ogg"
	$AudioStreamPlayer.stream = load(streamPath)
	$AudioStreamPlayer.volume_db = -20
	$AudioStreamPlayer.play()

func cleanup():
	var cryptGeneratorNodes = get_tree().get_nodes_in_group("crypt_generator")
	for node in cryptGeneratorNodes:
		node.destroy()

func create_crypt(newRandomCrypt = false):
	if newRandomCrypt:
		crypt_globals.cryptSeed = null
	var cryptGeneratorNode = null
	var cryptGeneratorNodes = get_tree().get_nodes_in_group("crypt_generator")
	for node in cryptGeneratorNodes:
		node.destroy()
	cryptGeneratorNode = cryptGenerator.instance()
	add_child(cryptGeneratorNode)
	cryptGeneratorNode.generate_crypt()

func exit_crypt():
	cleanup()
	crypt_globals.cryptSeed = null
	get_tree().change_scene("res://village/village.tscn")

func quit_to_main_menu():
	cleanup()
	get_tree().change_scene("res://ui/main_menu/main_menu.tscn")