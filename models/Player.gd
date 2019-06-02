extends Actor

class_name Player

var maxHealth : float
var health : float
var timeStart : int
var timeElapsed : int
var lightNode : Light2D
var debugInfo : CanvasLayer
var weapon : Area2D

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
		   lightNode : Light2D = null,
		   debugInfo : CanvasLayer = null,
		   weapon : Area2D = null) -> void:
	._init(instance, position, width, height, velocity, maxVelocity, walkingSpeed, sprintingSpeed, isSprinting)
	self.maxHealth = maxHealth
	self.health = health
	self.timeStart = timeStart
	self.timeElapsed = timeElapsed
	self.lightNode = lightNode
	self.debugInfo = debugInfo
	self.weapon = weapon

####################
# Helper Functions #
####################

func damage(amountToDamage : float) -> void:
	if self.health > 0:
		self.health -= amountToDamage

func die() -> void:
	if self.instance != null:
		self.instance.die()

func try_die() -> void:
	if self.health <= 0:
		self.die()

func update() -> void:
	try_die()