extends KinematicBody2D

var enemyModel = null

var remainingVelocity = Vector2.ZERO
var direction = Vector2.ZERO
var speed = Vector2.ZERO

###################
# Godot Functions #
###################

func _process(delta):
	if enemyModel != null:
		enemyModel.update()

func _physics_process(delta):
	if self.enemyModel != null:
		self.remainingVelocity = move_and_slide(self.direction * self.speed)
		self.direction = self.enemyModel.behave(delta, is_on_wall(), self.remainingVelocity)
		self.speed = self.enemyModel.speed if self.enemyModel.playerBody == null else self.enemyModel.huntingSpeed

func _on_Area2D_body_entered(body):
	if self.enemyModel != null:
		self.enemyModel.playerBody = body

func _on_Area2D_body_exited(body):
	if self.enemyModel != null:
		self.enemyModel.playerBody = null

####################
# Helper Functions #
####################

func damage(amountToDamage):
	if enemyModel != null:
		enemyModel.damage(amountToDamage)

func destroy():
	queue_free()
