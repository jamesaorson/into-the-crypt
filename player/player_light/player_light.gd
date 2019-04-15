extends Light2D

var initialScale = null

func _init():
	self.initialScale = float(texture_scale)

func update_size(delta):
	if texture_scale > 0:
		texture_scale -= (player_light_globals.timeScale * delta)
		if texture_scale <= 0.1:
			texture_scale = 0