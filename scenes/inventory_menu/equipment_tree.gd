extends InventoryTree

var rootTreeItem : TreeItem
var armorTreeItem : TreeItem
var weaponTreeItem : TreeItem
var vanityTreeItem : TreeItem

var selectedItem : TreeItem

onready var contextMenu : PopupMenu = $'..'/ContextMenu
onready var itemInfo : PanelContainer = $'..'/ItemInfo
onready var itemInfoLabel : RichTextLabel = $'..'/ItemInfo/RichTextLabel

const EQUIP : int = 0
const EXIT : int = 1

###################
# Godot Functions #
###################

func _ready() -> void:
	self.contextMenu.set_hide_on_window_lose_focus(false)

####################
# Helper Functions #
####################

func update_tree(items : Dictionary):
	self.clear()
	self.rootTreeItem = self.create_item(self)
	self.hide_root = true
	self.columns = 2

	self.armorTreeItem = self.create_item(self.rootTreeItem)
	self.armorTreeItem.set_text(0, 'Armor')
	utility.add_items_to_tree(
		self,
		self.armorTreeItem,
		items[inventory_globals.ITEM_TYPE.ARMOR]
	)

	self.weaponTreeItem = self.create_item(self.rootTreeItem)
	self.weaponTreeItem.set_text(0, 'Weapons')
	utility.add_items_to_tree(
		self,
		self.weaponTreeItem,
		items[inventory_globals.ITEM_TYPE.WEAPON]
	)

	self.vanityTreeItem = self.create_item(self.rootTreeItem)
	self.vanityTreeItem.set_text(0, 'Vanity')
	utility.add_items_to_tree(
		self,
		self.vanityTreeItem,
		items[inventory_globals.ITEM_TYPE.VANITY_EQUIPMENT]
	)

###################
# Signal handlers #
###################

func _on_Tree_item_rmb_selected(clickPosition : Vector2) -> void:
	self.selectedItem = self.get_selected()
	self.contextMenu.clear()
	self.contextMenu.rect_position = self.rect_global_position + clickPosition
	self.contextMenu.visible = true
	self.contextMenu.add_item('Equip', EQUIP)
	self.contextMenu.add_item('Exit', EXIT)

func _on_ContextMenu_id_pressed(ID : int):
	match ID:
		EQUIP:
			var weaponName : String = self.selectedItem.get_text(0)
			print('cell equipped ', weaponName, ' ', self.selectedItem.get_text(1))
			var player : PlayerNode = self.get_player()
			if player != null:
				player.equip_weapon(weaponName)
		EXIT:
			self.contextMenu.visible = false

func _on_Tree_cell_selected():
	self.show_item_info(self.itemInfoLabel)