extends ConsumableItem

class_name BuffConsumableItem

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
	inventory_globals.ITEM_TYPE.BUFF,
	info
) -> void:
	pass
