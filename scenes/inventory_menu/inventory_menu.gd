extends CanvasLayer

class_name InventoryMenu

onready var consumablesTree : Tree = $Control/Panel/Tabs/Consumables/TreeContainer/Tree
onready var equipmentTree : Tree = $Control/Panel/Tabs/Equipment/TreeContainer/Tree
onready var materialsTree : Tree = $Control/Panel/Tabs/Materials/TreeContainer/Tree

####################
# Helper Functions #
####################

func update_inventory(player : Player) -> void:
	self.consumablesTree.update_tree(player.consumables)
	self.equipmentTree.update_tree(player.equipment)
	self.materialsTree.update_tree(player.materials)
