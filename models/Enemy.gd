extends Actor

class_name Enemy

var friction = 0.9
var timeStart = null
var timeElapsed = null
var lightNode = null

func _init(instance = null,
		   position = Vector2(), 
		   width = 0, 
		   height = 0, 
		   velocity = Vector2(), 
		   maxVelocity = 0, 
		   walkingSpeed = 0, 
		   sprintingSpeed = 0, 
		   isSprinting = false,
		   timeStart = null,
		   timeElapsed = null,
		   lightNode = null):
	._init(instance, position, width, height, velocity, walkingSpeed, sprintingSpeed, isSprinting)
	self.timeStart = timeStart
	self.timeElapsed = timeElapsed
	self.lightNode = lightNode