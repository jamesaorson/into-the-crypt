extends Light2D

class_name PlayerLight

export(float) var timeScale : float = 0.01

var initialScale : float = 1.0
var isExtinguished : bool = false

###################
# Godot Functions #
###################

func _init() -> void:
	self.initialScale = float(texture_scale)

func _process(delta : float) -> void:
	if self.texture_scale > 0:
		self.isExtinguished = false
		self.texture_scale -= (timeScale * delta)
		if self.texture_scale <= 0.1:
			extinguish()

####################
# Helper Functions #
####################

func extinguish() -> void:
	self.texture_scale = 0.0
	self.isExtinguished = true