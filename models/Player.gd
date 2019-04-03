extends Actor

class_name Player

var maxHealth : float
var health : float
var timeStart : int
var timeElapsed : int
var weaponNode : Area2D

###################
# Godot Functions #
###################

func _init(instance : Node2D = null,
		   position : Vector2 = Vector2(), 
		   width : float = 0.0,
		   height : float = 0.0, 
		   velocity : Vector2 = Vector2(), 
		   maxVelocity : float = 0.0,
		   walkingSpeed : float = 0.0,
		   sprintingSpeed : float = 0.0,
		   isSprinting : bool = false,
		   maxHealth : float = 1.0,
		   health : float = 1.0,
		   timeStart : int = -1,
		   timeElapsed : int = -1,
		   weaponNode : Area2D = null) -> void:
	._init(instance, position, width, height, velocity, maxVelocity, walkingSpeed, sprintingSpeed, isSprinting)
	self.maxHealth = maxHealth
	self.health = health
	self.timeStart = timeStart
	self.timeElapsed = timeElapsed
	self.weaponNode = weaponNode

####################
# Helper Functions #
####################

func damage(amountToDamage : float) -> void:
	if self.health > 0:
		self.health -= amountToDamage

func die() -> bool:
	if self.instance != null:
		self.instance.die()
		return true
	return false

func try_die() -> bool:
	if self.health <= 0:
		return self.die()
	return false

func update() -> void:
	try_die()