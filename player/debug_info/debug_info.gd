extends CanvasLayer

export(String) var cryptSeedLabelPrefix = "Crypt Seed: "
export(String) var healthLabelPrefix = "Health: "
var player = null

###################
# Godot Functions #
###################

func _ready():
	self.update()

####################
# Helper Functions #
####################

func update():
	if crypt_globals.cryptSeed != null:
		$CryptSeedLabel.text = self.cryptSeedLabelPrefix + str(crypt_globals.cryptSeed)
		$CryptSeedLabel.update()
	if self.player != null:
		if self.player.health <= 0:
			$HealthLabel.text = "You are dead"
		else:
			$HealthLabel.text = self.healthLabelPrefix + str(self.player.health) + "/" + str(self.player.maxHealth)
		$HealthLabel.update()