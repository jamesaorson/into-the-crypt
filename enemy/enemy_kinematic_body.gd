extends KinematicBody2D

var enemyModel = null

###################
# Godot Functions #
###################

func _process(delta):
	if enemyModel != null:
		enemyModel.update()

func _on_EnemyKinematicBody2D_input_event(viewport, event, shape_idx):
	if event is InputEventMouseButton and event.button_index == 1 and event.is_pressed() and !event.is_echo():
    	enemyModel.damage(1)

####################
# Helper Functions #
####################

func destroy():
	queue_free()