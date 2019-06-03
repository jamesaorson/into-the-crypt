extends KinematicBody2D

class_name EnemyNode

var enemyModel : Enemy = null

var remainingVelocity : Vector2 = Vector2.ZERO
var direction : Vector2 = Vector2.ZERO
var speed : float = 0
var canAttack : bool = true
var inRangeToAttack : bool = false
var playerToAttack : PlayerNode = null

export(int) var attackTime : int = 1

###################
# Godot Functions #
###################

func _physics_process(delta : float) -> void:
	if self.enemyModel != null:
		self.remainingVelocity = move_and_slide(self.direction * self.speed)
		self.direction = self.enemyModel.behave(delta, is_on_wall(), self.remainingVelocity)
		self.speed = self.enemyModel.speed if self.enemyModel.player == null else self.enemyModel.huntingSpeed

func _process(delta : float) -> void:
	if self.enemyModel != null:
		self.enemyModel.update()
		if self.canAttack and self.inRangeToAttack and self.playerToAttack != null:
			attack()

func _ready() -> void:
	$AttackRange/AttackTimer.wait_time = attackTime

####################
# Helper Functions #
####################

func attack() -> void:
	if self.playerToAttack != null and self.enemyModel != null:
		$AttackRange/AttackTimer.start()
		self.canAttack = false
		self.playerToAttack.damage(self.enemyModel.calculate_attack())

func damage(amountToDamage : float) -> void:
	if self.enemyModel != null:
		self.enemyModel.damage(amountToDamage)

func destroy() -> void:
	queue_free()

###################
# Signal Handlers #
###################

func _on_AttackRange_body_entered(body : PlayerNode) -> void:
	self.inRangeToAttack = true
	self.playerToAttack = body

func _on_AttackRange_body_exited(body : PlayerNode) -> void:
	self.inRangeToAttack = false
	self.playerToAttack = null

func _on_AttackTimer_timeout() -> void:
	self.canAttack = true

func _on_HuntingRange_body_entered(body : PlayerNode) -> void:
	if self.enemyModel != null:
		self.enemyModel.player = body

func _on_HuntingRange_body_exited(body : PlayerNode) -> void:
	if self.enemyModel != null:
		self.enemyModel.player = null