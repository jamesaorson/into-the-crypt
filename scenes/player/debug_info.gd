extends CanvasLayer

class_name PlayerDebugInfo

export(String) var cryptSeedLabelPrefix : String = 'Crypt Seed: '
export(String) var healthLabelPrefix : String = 'Health: '
export(String) var weaponLabelPrefix : String = 'Weapon: '

###################
# Godot Functions #
###################

func _ready() -> void:
	self.update(null)

####################
# Helper Functions #
####################

func update(player : Player) -> void:
	if crypt_manager_globals.cryptSeed >= 0:
		$CryptSeed.text = self.cryptSeedLabelPrefix + str(crypt_manager_globals.cryptSeed)
		$CryptSeed.update()
	if player != null:
		if player.health <= 0:
			$Health.text = 'You are dead'
		else:
			$Health.text = self.healthLabelPrefix + str(player.health) + '/' + str(player.maxHealth)
		$Health.update()

		if player.weaponNode != null:
			$Weapon.text = self.weaponLabelPrefix + player.weaponNode.weapon.name + ' with damage of ' + str(player.weaponNode.weapon.damage)
		else:
			$Weapon.text = ''
		$Weapon.update()
