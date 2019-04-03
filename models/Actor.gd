extends Object

class_name Actor

#warning-ignore-all:unused_class_variable
var instance : Node2D
var position : Vector2
var width : float
var height : float
var velocity : Vector2
var maxVelocity : float
var walkingSpeed : float
var sprintingSpeed : float
var isSprinting : bool

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
		   isSprinting : bool = false) -> void:
	self.instance = instance
	self.position = position
	self.width = width
	self.height = height
	self.velocity = velocity
	self.maxVelocity = maxVelocity
	self.walkingSpeed = walkingSpeed
	self.sprintingSpeed = sprintingSpeed
	self.isSprinting = isSprinting