extends CanvasLayer

class_name PlayerDebugInfo

export(String) var cryptSeedLabelPrefix : String = "Crypt Seed: "
export(String) var healthLabelPrefix : String = "Health: "

var player : Player = null

###################
# Godot Functions #
###################

func _ready() -> void:
	self.update()

####################
# Helper Functions #
####################

func update() -> void:
	if crypt_globals.cryptSeed >= 0:
		$CryptSeedLabel.text = self.cryptSeedLabelPrefix + str(crypt_globals.cryptSeed)
		$CryptSeedLabel.update()
	if self.player != null:
		if self.player.health <= 0:
			$HealthLabel.text = "You are dead"
		else:
			$HealthLabel.text = self.healthLabelPrefix + str(self.player.health) + "/" + str(self.player.maxHealth)
		$HealthLabel.update()