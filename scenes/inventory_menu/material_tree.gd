extends InventoryTree

var rootTreeItem : TreeItem
var smithingTreeItem : TreeItem
var medicalTreeItem : TreeItem
var vanityTreeItem : TreeItem

var selectedItem : TreeItem

const USE : int = 0
const EXIT : int = 1

onready var contextMenu : PopupMenu = $'..'/ContextMenu
onready var itemInfo : PanelContainer = $'..'/ItemInfo
onready var itemInfoLabel : RichTextLabel = $'..'/ItemInfo/RichTextLabel

####################
# Helper Functions #
####################

func update_tree(items : Dictionary):
	self.clear()
	self.rootTreeItem = self.create_item(self)
	self.hide_root = true
	self.columns = 2

	self.smithingTreeItem = self.create_item(self.rootTreeItem)
	self.smithingTreeItem.set_text(0, 'Smithing')
	utility.add_items_to_tree(
		self,
		self.smithingTreeItem,
		items[inventory_globals.ITEM_TYPE.SMITHING]
	)

	self.medicalTreeItem = self.create_item(self.rootTreeItem)
	self.medicalTreeItem.set_text(0, 'Medical')
	utility.add_items_to_tree(
		self,
		self.medicalTreeItem,
		items[inventory_globals.ITEM_TYPE.MEDICAL]
	)

	self.vanityTreeItem = self.create_item(self.rootTreeItem)
	self.vanityTreeItem.set_text(0, 'Vanity')
	utility.add_items_to_tree(
		self,
		self.vanityTreeItem,
		items[inventory_globals.ITEM_TYPE.VANITY_MATERIAL]
	)

###################
# Signal handlers #
###################

func _on_Tree_item_rmb_selected(clickPosition : Vector2) -> void:
	self.selectedItem = self.get_selected()
	self.contextMenu.clear()
	self.contextMenu.rect_position = self.rect_global_position + clickPosition
	self.contextMenu.visible = true
	self.contextMenu.add_item('Use', USE)
	self.contextMenu.add_item('Exit', EXIT)

func _on_ContextMenu_id_pressed(ID : int):
	match ID:
		USE:
			print('use')
		EXIT:
			self.contextMenu.visible = false


func _on_Tree_cell_selected():
	self.show_item_info(self.itemInfoLabel)