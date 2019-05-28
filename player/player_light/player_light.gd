extends Light2D

var initialScale = null
export(float) var timeScale = 0.01

###################
# Godot Functions #
###################

func _init():
	self.initialScale = float(texture_scale)

####################
# Helper Functions #
####################

func update_size(delta):
	if texture_scale > 0:
		texture_scale -= (timeScale * delta)
		if texture_scale <= 0.1:
			texture_scale = 0