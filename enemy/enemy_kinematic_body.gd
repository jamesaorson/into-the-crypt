extends KinematicBody2D

var enemyModel = null

###################
# Godot Functions #
###################

func _process(delta):
	if enemyModel != null:
		enemyModel.update()

func damage(amountToDamage):
	if enemyModel != null:
		enemyModel.damage(amountToDamage)

####################
# Helper Functions #
####################

func destroy():
	queue_free()