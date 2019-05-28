extends Actor

class_name Enemy

var maxHealth
var health
var timeStart
var timeElapsed
var lightNode
var debugInfo
var direction
var huntingDirection
var playerBody

var speed = 100
var huntingSpeed = 300

const UP = Vector2(0, -1)
const DOWN = -UP
const LEFT = Vector2(-1, 0)
const RIGHT = -LEFT

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
	self.direction = Vector2.LEFT
	self.huntingDirection = Vector2.ZERO
	self.playerBody = null

####################
# Helper Functions #
####################

func behave(delta, isOnWall, remainingVelocity):
	if self.playerBody != null:
		self.huntingDirection = (self.playerBody.get_transform().origin - self.instance.get_transform().origin).normalized()
		if isOnWall:
			var dir = [1, -1]
			if abs(self.huntingDirection.x) > abs(self.huntingDirection.y):
				if self.huntingDirection.y > 0:
					self.huntingDirection.y += 0.05
				else:
					self.huntingDirection.y -= 0.05
			else:
				if self.huntingDirection.x > 0:
					self.huntingDirection.x += 0.05
				else:
					self.huntingDirection.x -= 0.05
		return self.huntingDirection
	elif isOnWall:
		self.huntingDirection = Vector2.ZERO
		match self.direction:
			UP:
				self.direction = RIGHT
			RIGHT:
				self.direction = DOWN
			DOWN:
				self.direction = LEFT
			LEFT:
				self.direction = UP
	return self.direction

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