extends EquipmentItem

class_name ArmorEquipmentItem

###################
# Godot Functions #
###################

func _init(
	name : String,
	quantity : int,
	info : String
).(
	name,
	quantity,
	inventory_globals.ITEM_TYPE.ARMOR,
	info
) -> void:
	pass
