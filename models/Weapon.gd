extends Object

var animationPlayer
var currentAttack
var damage
var numberOfAttacksInCombo
var currentAnimation

###################
# Godot Functions #
###################

func _init(animationPlayer = null,
		   currentAttack = 1,
		   damage = 0,
		   numberOfAttacksInCombo = 1,
		   startingAnimation = "_idle"):
	self.animationPlayer = animationPlayer
	self.currentAttack = currentAttack
	self.damage = damage
	self.numberOfAttacksInCombo = numberOfAttacksInCombo
	self.currentAnimation = startingAnimation

####################
# Helper Functions #
####################

func animation_finished(animationName, direction):
	if self.animationPlayer != null and self.animationPlayer.current_animation.empty():
		self.currentAttack = 1
		self.currentAnimation = "_idle"
		self.animationPlayer.play(direction + self.currentAnimation)

func attack(direction):
	if self.animationPlayer != null:
		self.currentAnimation = "_attack_" + str(self.currentAttack)
		self.animationPlayer.play(direction + self.currentAnimation)
		self.currentAttack = (self.currentAttack + 1) % (numberOfAttacksInCombo + 1)

func make_contact(body):
	body.damage(damage)

func update_direction(direction):
	if self.currentAnimation == "_idle":
		self.animationPlayer.play(direction + self.currentAnimation)