extends EquipmentItem

class_name VanityEquipmentItem

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
	inventory_globals.ITEM_TYPE.VANITY_EQUIPMENT,
	info
) -> void:
	pass
