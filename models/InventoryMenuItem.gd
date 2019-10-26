extends Object

class_name InventoryMenuItem

#warning-ignore-all:unused_class_variable
var name : String
var quantity : int
var inventoryType : int
var info : String

###################
# Godot Functions #
###################

func _init(
	name : String,
	quantity : int,
	inventoryType : int,
	info : String = ''
) -> void:
	self.name = name
	self.quantity = quantity
	self.inventoryType = inventoryType
	self.info = info
