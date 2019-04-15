extends Actor

class_name Player

var playerIndex
var friction
var timeStart
var timeElapsed
var lightNode
var debugInfo

func _init(instance = null,
		   position = Vector2(), 
		   width = 0, 
		   height = 0, 
		   velocity = Vector2(), 
		   walkingSpeed = 0, 
		   sprintingSpeed = 0, 
		   isSprinting = false, 
		   playerIndex = -1,
		   friction = 0,
		   timeStart = null,
		   timeElapsed = null,
		   lightNode = null,
		   debugInfo = null):
	._init(instance, position, width, height, velocity, walkingSpeed, sprintingSpeed, isSprinting)
	self.playerIndex = playerIndex
	self.friction = friction
	self.timeStart = timeStart
	self.timeElapsed = timeElapsed
	self.lightNode = lightNode
	self.debugInfo = debugInfo