extends Node

onready var cryptGenerator = load("res://crypt/crypt_generator/crypt_generator.tscn")

###################
# Godot Functions #
###################

func _process(delta):
	if Input.is_action_pressed("pause"):
		quit_game()
	elif Input.is_action_pressed("reset"):
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
	randomize()
	var cryptSeed = randi()
	cryptGeneratorNode.generate_crypt(cryptSeed)

func quit_game():
	get_tree().quit()