extends ConsumableItem

class_name ThrowableConsumableItem

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
	inventory_globals.ITEM_TYPE.THROWABLE,
	info
) -> void:
	pass
