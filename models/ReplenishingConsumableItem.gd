extends ConsumableItem

class_name ReplenishingConsumableItem

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
	inventory_globals.ITEM_TYPE.REPLENISHING,
	info
) -> void:
	pass