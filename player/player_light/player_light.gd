extends Light2D

var initialScale = null
export(float) var timeScale = 0.01
var isExtinguished = false

###################
# Godot Functions #
###################

func _init():
	self.initialScale = float(texture_scale)

func _process(delta):
	if self.texture_scale > 0:
		self.isExtinguished = false
		self.texture_scale -= (timeScale * delta)
		if self.texture_scale <= 0.1:
			extinguish()

####################
# Helper Functions #
####################

func extinguish():
	self.texture_scale = 0
	self.isExtinguished = true