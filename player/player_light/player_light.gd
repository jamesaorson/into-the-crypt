extends Light2D

var initialScale = null

func _init():
	self.initialScale = float(texture_scale)

func update_size(timeElapsed):
	if timeElapsed > 0 and texture_scale > 0:
		texture_scale = self.initialScale / max((float(timeElapsed) * player_light_globals.timeScale), self.initialScale)
		if texture_scale <= 0.1:
			texture_scale = 0