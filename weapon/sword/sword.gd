extends Area2D

var Weapon : Resource = load("res://models/Weapon.gd")

onready var animationPlayer : AnimationPlayer = $AnimationPlayer
export var canAttack : bool = false

export(float) var damage : float = 1
export(int) var numberOfAttacksInCombo : int = 2
var direction : String = "right"

var weapon : Weapon

###################
# Godot Functions #
###################

func _ready() -> void:
	self.weapon = Weapon.new(self.animationPlayer, 1, self.damage, self.numberOfAttacksInCombo)

####################
# Helper Functions #
####################

func attack() -> void:
	if self.canAttack:
		self.canAttack = false
		self.weapon.attack(self.direction)

func flip_h() -> void:
	$Sprite.set_flip_h($Sprite.flip_h)
	if self.direction == "right":
		self.direction = "left"
	else:
		self.direction = "right"
	self.weapon.update_direction(direction)

###################
# Signal Handlers #
###################

func _on_Area2D_body_entered(body : KinematicBody2D) -> void:
	self.weapon.make_contact(body)

func _on_AnimationPlayer_animation_finished(animationName : String) -> void:
	self.weapon.animation_finished(animationName, self.direction)