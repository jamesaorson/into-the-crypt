extends Object

class_name Weapon

var animationPlayer : AnimationPlayer
var currentAttack : int
var damage : float
var numberOfAttacksInCombo : int
var currentAnimation : String

###################
# Godot Functions #
###################

func _init(animationPlayer : AnimationPlayer = null,
		   currentAttack : int = 1,
		   damage : float = 0.0,
		   numberOfAttacksInCombo : int = 1,
		   startingAnimation : String = "_idle") -> void:
	self.animationPlayer = animationPlayer
	self.currentAttack = currentAttack
	self.damage = damage
	self.numberOfAttacksInCombo = numberOfAttacksInCombo
	self.currentAnimation = startingAnimation

####################
# Helper Functions #
####################

func animation_finished(animationName : String, direction : String) -> void:
	if self.animationPlayer != null and self.animationPlayer.current_animation.empty():
		self.currentAttack = 1
		self.currentAnimation = "_idle"
		self.animationPlayer.play(direction + self.currentAnimation)

func attack(direction : String) -> void:
	if self.animationPlayer != null:
		self.currentAnimation = "_attack_" + str(self.currentAttack)
		self.animationPlayer.play(direction + self.currentAnimation)
		self.currentAttack = (self.currentAttack + 1) % (numberOfAttacksInCombo + 1)

func make_contact(body : KinematicBody2D) -> void:
	body.damage(self.damage)

func update_direction(direction : String) -> void:
	if self.currentAnimation == "_idle":
		self.animationPlayer.play(direction + self.currentAnimation)