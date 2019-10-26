extends MaterialItem

class_name SmithingMaterialItem

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
	inventory_globals.ITEM_TYPE.SMITHING,
	info
) -> void:
	pass
