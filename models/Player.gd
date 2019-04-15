extends Actor

class_name Player

var playerIndex
var maxHealth
var health
var timeStart
var timeElapsed
var lightNode
var debugInfo

###################
# Godot Functions #
###################

func _init(instance = null,
		   position = Vector2(), 
		   width = 0, 
		   height = 0, 
		   velocity = Vector2(), 
		   maxVelocity = 0, 
		   walkingSpeed = 0, 
		   sprintingSpeed = 0, 
		   isSprinting = false, 
		   playerIndex = -1,
		   maxHealth = 1,
		   health = 1,
		   timeStart = null,
		   timeElapsed = null,
		   lightNode = null,
		   debugInfo = null):
	._init(instance, position, width, height, velocity, maxVelocity, walkingSpeed, sprintingSpeed, isSprinting)
	self.playerIndex = playerIndex
	self.maxHealth = maxHealth
	self.health = health
	self.timeStart = timeStart
	self.timeElapsed = timeElapsed
	self.lightNode = lightNode
	self.debugInfo = debugInfo