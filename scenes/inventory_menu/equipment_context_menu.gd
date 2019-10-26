extends MenuButton

const EQUIP : int = 0
const Exit : int = 1

###################
# Signal handlers #
###################

func _on_ContextMenu_pressed():
	var popup : PopupMenu = self.get_popup()
	popup.add_item('Equip', EQUIP)
	popup.add_item('Exit', Exit)
	popup.connect('id_pressed', self, '_on_id_pressed')

func _on_id_pressed(id : int) -> void:
	print('id ', id)