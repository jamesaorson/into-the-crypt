extends Node

#warning-ignore-all:unused_class_variable
export var UI_ACCEPT : String = 'ui_accept'
export var UI_CANCEL : String = 'ui_cancel'

export var PRIMARY_ATTACK : String = 'primary_attack'

export var UP : String = 'up'
export var DOWN : String = 'down'
export var LEFT : String = 'left'
export var RIGHT : String = 'right'

export var PAUSE : String = 'pause'
export var RESET : String = 'reset'
export var SPRINT : String = 'sprint'
export var TOGGLE_DEBUG : String = 'toggle_debug'
export var INVENTORY_TOGGLE : String = 'inventory_toggle'

func get_input_display_names(input) -> Array:
	var inputs : Array = []
	var actionList = InputMap.get_action_list(input)
	for action in actionList:
		inputs.append(action.as_text())
	return inputs