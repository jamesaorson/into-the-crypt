extends EquipmentItem

class_name WeaponEquipmentItem

var animationPlayer : AnimationPlayer
var currentAttack : int
var damage : float
var numberOfAttacksInCombo : int
var currentAnimation : String

###################
# Godot Functions #
###################

func _init(
	name : String,
	quantity : int,
	info : String,
	animationPlayer : AnimationPlayer = null,
	currentAttack : int = 0,
	damage : float = 0.0,
	numberOfAttacksInCombo : int = 0,
	startingAnimation : String = '_idle'
).(
	name,
	quantity,
	inventory_globals.ITEM_TYPE.WEAPON,
	info
) -> void:
	self.animationPlayer = animationPlayer
	self.currentAttack = currentAttack
	self.damage = damage
	self.name = name
	self.numberOfAttacksInCombo = numberOfAttacksInCombo
	self.currentAnimation = startingAnimation

####################
# Helper Functions #
####################

func animation_finished(_animationName : String, direction : String) -> void:
	if self.animationPlayer != null and self.animationPlayer.current_animation.empty():
		self.currentAttack = 0
		self.currentAnimation = '_idle'
		self.animationPlayer.play(direction + self.currentAnimation)

func attack(direction : String) -> void:
	if self.animationPlayer != null:
		self.currentAnimation = '_attack_' + str(self.currentAttack)
		self.animationPlayer.play(direction + self.currentAnimation)
		self.currentAttack += 1
		if numberOfAttacksInCombo == 0:
			self.currentAttack = 0
		else:
			self.currentAttack %= numberOfAttacksInCombo

func make_contact(body : KinematicBody2D) -> void:
	body.damage(self.damage)

func update_direction(direction : String) -> void:
	if self.currentAnimation == '_idle':
		self.animationPlayer.play(direction + self.currentAnimation)