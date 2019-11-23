extends Node

#warning-ignore-all:unused_class_variable
enum ITEM_TYPE {
    CONSUMABLE,
    EQUIPMENT,
    MATERIAL,

    # Consumable
    BUFF,
    REPLENISHING,
    THROWABLE,

    # Equipment
    ARMOR,
    VANITY_EQUIPMENT,
    WEAPON,

    # Material
    MEDICAL,
    SMITHING,
    VANITY_MATERIAL
}