extends MaterialItem

class_name MedicalMaterialItem

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
	inventory_globals.ITEM_TYPE.MEDICAL,
	info
) -> void:
	pass
