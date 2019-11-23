extends Actor

class_name Enemy

var maxHealth : float
var health : float
var timeStart : int
var timeElapsed : int
var lightNode : Light2D
var direction : Vector2
var huntingDirection : Vector2
var player : PlayerNode

#warning-ignore:unused_class_variable
export(float) var speed : float = 100
#warning-ignore:unused_class_variable
export(float) var huntingSpeed : float = 300
export(float) var attackDamage : float = 1

const UP : Vector2 = Vector2(0, -1)
const DOWN : Vector2 = -UP
const LEFT : Vector2 = Vector2(-1, 0)
const RIGHT : Vector2 = -LEFT

###################
# Godot Functions #
###################

func _init(
	instance : Node2D = null,
	position : Vector2 = Vector2(), 
	width : float = 0, 
	height : float = 0,
	velocity = Vector2(),
	maxVelocity : float = 0,
	walkingSpeed : float = 0, 
	sprintingSpeed : float = 0, 
	isSprinting = false,
	maxHealth : float = 1.0,
	health : float  = 1.0,
	timeStart : int = -1,
	timeElapsed : int = -1,
	lightNode : Light2D = null,
	attackDamage : int = 1
).(
	instance,
	position,
	width,
	height,
	velocity,
	maxVelocity,
	walkingSpeed,
	sprintingSpeed,
	isSprinting
) -> void:
	self.timeStart = timeStart
	self.timeElapsed = timeElapsed
	self.lightNode = lightNode
	self.maxHealth = maxHealth
	self.health = health
	self.direction = Vector2.LEFT
	self.huntingDirection = Vector2.ZERO
	self.player = null
	self.attackDamage = attackDamage

####################
# Helper Functions #
####################

func behave(_delta : float, isOnWall : bool, _remainingVelocity : Vector2) -> Vector2:
	if self.player != null:
		self.huntingDirection = (self.player.get_transform().origin - self.instance.get_transform().origin).normalized()
		if isOnWall:
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

func calculate_attack() -> float:
	return self.attackDamage

func damage(amountToDamage) -> void:
	if self.health > 0:
		self.health -= amountToDamage 

func die() -> bool:
	if self.instance != null and crypt_manager_globals.enemies.has(get_instance_id()):
		var _hadKey : bool = crypt_manager_globals.enemies.erase(get_instance_id())
		self.instance.queue_free()
		return true
	return false

func heal(amountToHeal) -> void:
	self.health = min(self.maxHealth, self.health + amountToHeal)

func try_die() -> bool:
	if self.health <= 0:
		return self.die()
	return false

func update() -> void:
	if self.try_die():
		print('Enemy died')