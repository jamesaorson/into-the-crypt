extends InventoryMenuItem

class_name EquipmentItem

var isEquipped : bool

###################
# Godot Functions #
###################

func _init(
	name : String,
	quantity : int,
	inventoryType : int,
	info : String,
	isEquipped : bool = false
).(
	name,
	quantity,
	inventoryType,
	info
) -> void:
	self.isEquipped = isEquipped
