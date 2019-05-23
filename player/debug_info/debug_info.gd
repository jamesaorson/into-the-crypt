extends CanvasLayer

func _ready():
	if crypt_globals.cryptSeed != null:
		$CryptSeedLabel.text = "Crypt Seed: " + str(crypt_globals.cryptSeed)
	$CryptSeedLabel.update()

func update_info():
	$CryptSeedLabel.update()