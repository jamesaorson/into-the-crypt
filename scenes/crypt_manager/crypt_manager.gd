extends Node

###################
# Godot Functions #
###################

func _input(_event : InputEvent) -> void:
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
	$AudioStreamPlayer.volume_db = volume
	$AudioStreamPlayer.play()

func create_crypt(newRandomCrypt : bool = false) -> void:
	if newRandomCrypt:
		crypt_manager_globals.cryptSeed = -1
	$Crypt/CryptTileMap.generate_crypt()

func exit_crypt() -> void:
	crypt_manager_globals.cryptSeed = -1
	utility.error_handled_scene_change('village')