extends MaterialItem

class_name VanityMaterialItem

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
	inventory_globals.ITEM_TYPE.VANITY_MATERIAL,
	info
) -> void:
	pass
