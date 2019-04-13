extends CanvasLayer

onready var cryptSeedLabel = get_node("CryptSeedLabel")
onready var lightScaleLabel = get_node("LightScaleLabel")

func _ready():
	cryptSeedLabel.text = "Crypt Seed: " + str(crypt_globals.cryptSeed)
	cryptSeedLabel.update()

func update_info():
	cryptSeedLabel.update()