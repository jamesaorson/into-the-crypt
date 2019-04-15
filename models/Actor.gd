extends Object

class_name Actor

var id
var instance
var position
var width
var height
var velocity
var maxVelocity
var walkingSpeed
var sprintingSpeed
var isSprinting

func _init(instance = null,
		   position = Vector2(), 
		   width = 0, 
		   height = 0, 
		   velocity = Vector2(), 
		   maxVelocity = 0, 
		   walkingSpeed = 0, 
		   sprintingSpeed = 0, 
		   isSprinting = false):
	self.instance = instance
	self.position = position
	self.width = width
	self.height = height
	self.velocity = velocity
	self.maxVelocity = maxVelocity
	self.walkingSpeed = walkingSpeed
	self.sprintingSpeed = sprintingSpeed
	self.isSprinting = isSprinting