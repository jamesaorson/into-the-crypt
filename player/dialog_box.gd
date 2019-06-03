extends Polygon2D

var player : PlayerNode = null

###################
# Godot Functions #
###################

func _ready() -> void:
	set_process_input(true)

func _input(event : InputEvent) -> void:
	if self.visible and self.player != null and self.player.isTalking:
		if Input.is_action_just_pressed(input_globals.UI_ACCEPT) or Input.is_action_pressed(input_globals.UI_CANCEL):
			self.player.isTalking = false
			self.player.canTalkWithVillager = false
			self.visible = false

####################
# Helper Functions #
####################

func talk(player : PlayerNode, villagerToTalkTo : Area2D):
	self.visible = true
	self.player = player
	self.player.canTalkWithVillager = false
	$Title.parse_bbcode(villagerToTalkTo.villagerName)
	$Content.parse_bbcode(villagerToTalkTo.greeting)