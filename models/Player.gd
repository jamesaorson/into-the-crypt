extends Actor

class_name Player

var equippedWeapon : WeaponEquipmentItem
var health : float
var maxHealth : float
var timeElapsed : int
var timeStart : int
var weaponNode : Area2D

var consumables : Dictionary
var equipment : Dictionary
var materials : Dictionary

###################
# Godot Functions #
###################

func _init(
	instance : Node2D = null,
	position : Vector2 = Vector2(), 
	width : float = 0.0,
	height : float = 0.0, 
	velocity : Vector2 = Vector2(), 
	maxVelocity : float = 0.0,
	walkingSpeed : float = 0.0,
	sprintingSpeed : float = 0.0,
	isSprinting : bool = false,
	maxHealth : float = 1.0,
	health : float = 1.0,
	timeStart : int = -1,
	timeElapsed : int = -1,
	weaponNode : Area2D = null
).(
	instance,
	position,
	width,
	height,
	velocity,
	maxVelocity,
	walkingSpeed,
	sprintingSpeed,
	isSprinting
) -> void:
	self.equippedWeapon = null
	self.maxHealth = maxHealth
	self.health = health
	self.timeStart = timeStart
	self.timeElapsed = timeElapsed
	self.weaponNode = weaponNode
	self.consumables = {}
	self.equipment = {}
	self.materials = {}
	self.fill_dummy_inventory()

####################
# Helper Functions #
####################

func add_item(item : InventoryMenuItem) -> void:
	self.set_item(item, true)

func damage(amountToDamage : float) -> void:
	if self.health > 0:
		self.health -= amountToDamage

func die() -> bool:
	if self.instance != null:
		self.instance.die()
		return true
	return false

func equip_weapon(weaponNode : Area2D) -> bool:
	var weaponItem : WeaponEquipmentItem = weaponNode.weapon
	if weaponItem == null:
		return false
	else:
		if self.equippedWeapon != null:
			self.equippedWeapon.isEquipped = false
		self.equippedWeapon = weaponItem
		self.equippedWeapon.isEquipped = true
		if self.weaponNode != null:
			self.weaponNode.queue_free()
		self.weaponNode = weaponNode
	return true

func fill_dummy_inventory() -> void:
	fill_dummy_consumables()
	fill_dummy_equipment()
	fill_dummy_materials()

func fill_dummy_consumables() -> void:
	self.add_item(
		ReplenishingConsumableItem.new('Rejuvenating Thing', 1, 'Rejuvenates you in some way.')
	)
	self.add_item(
		ReplenishingConsumableItem.new('Healing Thing', 2, 'Heals you in some way.')
	)

	self.add_item(
		BuffConsumableItem.new('Burning Thing', 1, 'Burns a hell of a lot.')
	)
	self.add_item(
		BuffConsumableItem.new('Acidic Thing', 2, 'Burns in an incredibly bad way.')
	)

	self.add_item(
		ThrowableConsumableItem.new('Molotov', 5, 'Gulag classic.')
	)
	self.add_item(
		ThrowableConsumableItem.new('Rock', 10, 'Caveman classic.')
	)

func fill_dummy_equipment() -> void:
	self.add_item(
		ArmorEquipmentItem.new('Chain Mail', 1, 'Clanky boi.')
	)
	self.add_item(
		ArmorEquipmentItem.new('Leggies', 2, 'White girl move.')
	)

	self.add_item(
		WeaponEquipmentItem.new(weapon_globals.AXE_NAME, 1, weapon_globals.AXE_INFO)
	)
	self.add_item(
		WeaponEquipmentItem.new(weapon_globals.SWORD_NAME, 1, weapon_globals.SWORD_INFO)
	)

	self.add_item(
		VanityEquipmentItem.new('Top Hat', 5, 'Dapper daddy.')
	)
	self.add_item(
		VanityEquipmentItem.new('Mustache', 10, 'Movember vibe check.')
	)

func fill_dummy_materials() -> void:
	self.add_item(
		SmithingMaterialItem.new('Iron Ore', 10, 'First meaningful step in Minecraft.')
	)
	self.add_item(
		SmithingMaterialItem.new('Gold Ore', 2, 'Butter.')
	)

	self.add_item(
		MedicalMaterialItem.new('Bandage', 5, 'Left 4 Dead WOULD BE proud.')
	)
	self.add_item(
		MedicalMaterialItem.new('Venom', 2, 'You can drink this!.')
	)

	self.add_item(
		VanityMaterialItem.new('Cloth', 5, 'Now you can make a sustainable diaper.')
	)
	self.add_item(
		VanityMaterialItem.new('Velvet', 10, 'Nice rope... *vsssh vssssssssh*')
	)

func get_inventory_item(itemName : String) -> InventoryMenuItem:
	var item : InventoryMenuItem
	
	item = self.get_consumable(itemName)
	if item != null:
		return item
	item = self.get_equipment(itemName)
	if item != null:
		return item
	item = self.get_material(itemName)
	return item

func get_item(itemType : int, itemName : String) -> InventoryMenuItem:
	var collection : Dictionary = {}
	match itemType:
		inventory_globals.ITEM_TYPE.BUFF, inventory_globals.ITEM_TYPE.REPLENISHING, inventory_globals.ITEM_TYPE.THROWABLE:
			collection = self.consumables[itemType]
		inventory_globals.ITEM_TYPE.ARMOR, inventory_globals.ITEM_TYPE.VANITY_EQUIPMENT, inventory_globals.ITEM_TYPE.WEAPON:
			collection = self.equipment[itemType]
		inventory_globals.ITEM_TYPE.MEDICAL, inventory_globals.ITEM_TYPE.SMITHING, inventory_globals.ITEM_TYPE.VANITY_MATERIAL:
			collection = self.materials[itemType]
	
	return collection.get(itemName)

# Consumables
func get_consumable(itemName : String) -> ConsumableItem:
	var item : ConsumableItem

	item = self.get_buff(itemName)
	if item != null:
		return item
	item = self.get_replenishing(itemName)
	if item != null:
		return item
	item = self.get_throwable(itemName)
	return item
	
func get_buff(itemName : String) -> BuffConsumableItem:
	var item : BuffConsumableItem = self.get_item(
		inventory_globals.ITEM_TYPE.BUFF,
		itemName
	)
	return item

func get_replenishing(itemName : String) -> ReplenishingConsumableItem:
	var item : ReplenishingConsumableItem = self.get_item(
		inventory_globals.ITEM_TYPE.REPLENISHING,
		itemName
	)
	return item

func get_throwable(itemName : String) -> ThrowableConsumableItem:
	var item : ThrowableConsumableItem = self.get_item(
		inventory_globals.ITEM_TYPE.THROWABLE,
		itemName
	)
	return item

# Equipment
func get_equipment(itemName : String) -> EquipmentItem:
	var item : EquipmentItem

	item = self.get_armor(itemName)
	if item != null:
		return item
	item = self.get_vanity_equipment(itemName)
	if item != null:
		return item
	item = self.get_weapon(itemName)
	return item
	
func get_armor(itemName : String) -> ArmorEquipmentItem:
	var item : ArmorEquipmentItem = self.get_item(
		inventory_globals.ITEM_TYPE.ARMOR,
		itemName
	)
	return item

func get_vanity_equipment(itemName : String) -> VanityEquipmentItem:
	var item : VanityEquipmentItem = self.get_item(
		inventory_globals.ITEM_TYPE.VANITY_EQUIPMENT,
		itemName
	)
	return item

func get_weapon(itemName : String) -> WeaponEquipmentItem:
	var item : WeaponEquipmentItem = self.get_item(
		inventory_globals.ITEM_TYPE.WEAPON,
		itemName
	)
	return item

# Materials
func get_material(itemName : String) -> MaterialItem:
	var item : MaterialItem

	item = self.get_medical_material(itemName)
	if item != null:
		return item
	item = self.get_smithing_material(itemName)
	if item != null:
		return item
	item = self.get_vanity_material(itemName)
	return item
	
func get_medical_material(itemName : String) -> MedicalMaterialItem:
	var item : MedicalMaterialItem = self.get_item(
		inventory_globals.ITEM_TYPE.MEDICAL,
		itemName
	)
	return item

func get_smithing_material(itemName : String) -> SmithingMaterialItem:
	var item : SmithingMaterialItem = self.get_item(
		inventory_globals.ITEM_TYPE.SMITHING,
		itemName
	)
	return item

func get_vanity_material(itemName : String) -> VanityMaterialItem:
	var item : VanityMaterialItem = self.get_item(
		inventory_globals.ITEM_TYPE.VANITY_MATERIAL,
		itemName
	)
	return item

func set_item(item : InventoryMenuItem, shouldAdd = false):
	var itemCollection : Dictionary = {}
	match item.inventoryType:
		# Consumable
		inventory_globals.ITEM_TYPE.BUFF, inventory_globals.ITEM_TYPE.REPLENISHING, inventory_globals.ITEM_TYPE.THROWABLE:
			itemCollection = self.consumables
		# Equipment
		inventory_globals.ITEM_TYPE.ARMOR, inventory_globals.ITEM_TYPE.VANITY_EQUIPMENT, inventory_globals.ITEM_TYPE.WEAPON:
			itemCollection = self.equipment
		# Material
		inventory_globals.ITEM_TYPE.MEDICAL, inventory_globals.ITEM_TYPE.SMITHING, inventory_globals.ITEM_TYPE.VANITY_MATERIAL:
			itemCollection = self.materials
		# Invalid
		_:
			print('Invalid inventory type')
			return
	var oldQuantity : int = 0

	if itemCollection.has(item.inventoryType):
		if itemCollection[item.inventoryType].has(item.name):
			if shouldAdd:
				oldQuantity = itemCollection[item.inventoryType][item.name].quantity
	else:
		itemCollection[item.inventoryType] = {}
	itemCollection[item.inventoryType][item.name] = item
	itemCollection[item.inventoryType][item.name].quantity += oldQuantity

func try_die() -> bool:
	if self.health <= 0:
		return self.die()
	return false

func update() -> void:
	if try_die():
		print('Player died!')
