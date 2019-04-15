extends Actor

class_name Enemy

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
		   maxHealth = 1,
		   health = 1,
		   timeStart = null,
		   timeElapsed = null,
		   lightNode = null,
		   debugInfo = null):
	._init(instance, position, width, height, velocity, walkingSpeed, sprintingSpeed, isSprinting)
	self.timeStart = timeStart
	self.timeElapsed = timeElapsed
	self.lightNode = lightNode
	self.maxHealth = maxHealth
	self.health = health
	self.debugInfo = debugInfo

####################
# Helper Functions #
####################

func damage(amountToDamage):
	self.health -= amountToDamage 

func heal(amountToHeal):
	self.health = min(self.maxHealth, self.health + amountToHeal)

func kill():
	if self.instance != null and crypt_globals.enemies.has(get_instance_id()):
		crypt_globals.enemies.erase(get_instance_id())
		self.instance.queue_free()

func try_kill():
	if self.health <= 0:
		self.kill()

func update():
	self.try_kill()