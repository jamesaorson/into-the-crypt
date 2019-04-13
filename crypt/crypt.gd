extends Node

onready var cryptGenerator = load("res://crypt/crypt_generator/crypt_generator.tscn")

###################
# Godot Functions #
###################

var shouldGenerate = false

func _process(delta):
	if Input.is_action_pressed("pause"):
		quit_to_main_menu()
	if Input.is_action_pressed("reset"):
		create_crypt()

func _ready():
	set_process(true)
	create_crypt()

####################
# Helper Functions #
####################

func create_crypt():
	var cryptGeneratorNode = null
	var cryptGeneratorNodes = get_tree().get_nodes_in_group("crypt_generator")
	if len(cryptGeneratorNodes) != 0:
		cryptGeneratorNode = cryptGeneratorNodes[0]
	else:
		cryptGeneratorNode = cryptGenerator.instance()
		get_tree().root.add_child(cryptGeneratorNode)
	cryptGeneratorNode.generate_crypt()

func quit_to_main_menu():
	var cryptGeneratorNodes = get_tree().get_nodes_in_group("crypt_generator")
	for cryptGeneratorNode in cryptGeneratorNodes:
		cryptGeneratorNode.destroy()
	get_tree().change_scene("res://ui/main_menu/main_menu.tscn")