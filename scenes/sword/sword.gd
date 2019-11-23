extends Area2D

onready var animationPlayer : AnimationPlayer = $AnimationPlayer
export var canAttack : bool = false

export(float) var damage : float = 1
export(int) var numberOfAttacksInCombo : int = 2
var direction : String = 'right'

var weapon : WeaponEquipmentItem

###################
# Godot Functions #
###################

func _ready() -> void:
	self.weapon.animationPlayer = self.animationPlayer

####################
# Helper Functions #
####################

func attack() -> void:
	if self.canAttack:
		self.canAttack = false
		self.weapon.attack(self.direction)

func flip_h() -> void:
	$Sprite.set_flip_h($Sprite.flip_h)
	if self.direction == 'right':
		self.direction = 'left'
	else:
		self.direction = 'right'
	self.weapon.update_direction(direction)

func setup(weapon : WeaponEquipmentItem) -> void:
	self.weapon = weapon
	self.weapon.animationPlayer = self.animationPlayer
	self.weapon.currentAttack = 0
	self.weapon.damage = self.damage
	self.weapon.numberOfAttacksInCombo = self.numberOfAttacksInCombo

###################
# Signal Handlers #
###################

func _on_AnimationPlayer_animation_finished(animationName : String) -> void:
	self.weapon.animation_finished(animationName, self.direction)

func _on_Area2D_body_entered(body : KinematicBody2D) -> void:
	self.weapon.make_contact(body)