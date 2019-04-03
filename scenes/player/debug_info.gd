extends CanvasLayer

class_name PlayerDebugInfo

export(String) var cryptSeedLabelPrefix : String = "Crypt Seed: "
export(String) var healthLabelPrefix : String = "Health: "
export(String) var weaponLabelPrefix : String = "Weapon: "

var player : Player = player_globals.player

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
		$CryptSeed.text = self.cryptSeedLabelPrefix + str(crypt_globals.cryptSeed)
		$CryptSeed.update()
	if self.player != null:
		if self.player.health <= 0:
			$Health.text = "You are dead"
		else:
			$Health.text = self.healthLabelPrefix + str(self.player.health) + "/" + str(self.player.maxHealth)
		$Health.update()

		if self.player.weaponNode != null:
			$Weapon.text = self.weaponLabelPrefix + self.player.weaponNode.weapon.name + " with damage of " + str(self.player.weaponNode.weapon.damage)
		else:
			$Weapon.text = ''
		$Weapon.update()
