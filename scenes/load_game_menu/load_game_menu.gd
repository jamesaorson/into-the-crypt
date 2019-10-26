extends Control

onready var save_file_button_scene : Resource = load(utility.construct_scene_path('save_file_button'))

###################
# Godot Functions #
###################

func _ready() -> void:
	$Menu/ReturnButton.grab_focus()
	var button_infos : Array = get_save_files()
	for button_info in button_infos:
		var save_file_button : SaveFileButton = save_file_button_scene.instance()
		save_file_button.text = button_info['text']
		save_file_button.connect('button_up', self, '_on_save_file_click', [save_file_button])
		$Menu/SaveFiles/SaveFilePanel/SaveFileList.add_child(save_file_button)

####################
# Helper Functions #
####################

func get_save_files() -> Array:
	return [
		{
			'text': 'Save File 1'
		},
		{
			'text': 'Save File 2'
		}
	]

func main_menu() -> void:
	utility.error_handled_scene_change('main_menu')

###################
# Signal handlers #
###################

signal save_file_click

func _on_save_file_click(save_file : SaveFileButton):
	print(save_file.text)

func _on_ReturnButton_pressed():
	main_menu()
