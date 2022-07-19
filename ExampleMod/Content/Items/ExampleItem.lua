import("Terraria")
import("Terraria.ModLoader")

local item

function contentType()
    return "modItem"
end

function setStaticDefaults()
    item.width = 20
    item.height = 20
    item.maxStack = 999
    item.value = Item.buyPrice(0, 0, 1, 0)
end

function setDefaults()

end

function addRecipes()

end
