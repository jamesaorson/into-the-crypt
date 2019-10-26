extends Tree

class_name InventoryTree

####################
# Helper Functions #
####################

func get_player(): # Returns PlayerNode. Cannot static type because cyclic dependency
	var player
	var players : Array = get_tree().get_nodes_in_group('player')
	if len(players) > 0:
		player = players[0]
	return player

func show_item_info(label : RichTextLabel) -> void:
	self.selectedItem = self.get_selected()
	var player = self.get_player()
	if player != null:
		var item : InventoryMenuItem = player.player.get_inventory_item(self.selectedItem.get_text(0))
		if item != null:
			label.text = item.info
		else:
			label.text = ''
