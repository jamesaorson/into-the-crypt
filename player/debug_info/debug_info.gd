extends CanvasLayer

onready var cryptSeedLabel = get_node("CryptSeedLabel")

func _ready():
	cryptSeedLabel.text = "Crypt Seed: " + str(crypt_globals.cryptSeed)
	cryptSeedLabel.update()

func update_info():
	cryptSeedLabel.update()