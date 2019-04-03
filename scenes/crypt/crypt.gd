extends Node

###################
# Godot Functions #
###################

func _input(event : InputEvent) -> void:
	if Input.is_action_pressed(input_globals.PAUSE):
		quit_to_main_menu()
	if OS.is_debug_build() and Input.is_action_pressed(input_globals.RESET):
		create_crypt(true)

func _ready() -> void:
	set_process(true)
	set_process_input(true)
	create_crypt()
	change_music()

####################
# Helper Functions #
####################

func change_music(streamPath : String = '', volume : float = -20) -> void:
	if streamPath == null or streamPath.empty():
		var track : int = int(rand_seed(OS.get_ticks_msec())[0]) % 2
		streamPath = 'res://assets/audio/bgm/passive_crypt_' + str(track) + '.ogg'
	$AudioStreamPlayer.stream = load(streamPath)
	$AudioStreamPlayer.volume_db = -20
	$AudioStreamPlayer.play()

func create_crypt(newRandomCrypt : bool = false) -> void:
	if newRandomCrypt:
		crypt_globals.cryptSeed = -1
	$CryptGenerator/CryptTileMap.generate_crypt()

func exit_crypt() -> void:
	crypt_globals.cryptSeed = -1
	get_tree().change_scene(utility.construct_scene_path('village'))

func quit_to_main_menu() -> void:
	get_tree().change_scene(utility.construct_scene_path('main_menu'))