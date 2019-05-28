extends CanvasLayer

export(String) var cryptSeedLabelPrefix = "Crypt Seed: "

###################
# Godot Functions #
###################

func _ready():
	if crypt_globals.cryptSeed != null:
		$CryptSeedLabel.text = cryptSeedLabelPrefix + str(crypt_globals.cryptSeed)
	$CryptSeedLabel.update()

####################
# Helper Functions #
####################

func update_info():
	$CryptSeedLabel.update()