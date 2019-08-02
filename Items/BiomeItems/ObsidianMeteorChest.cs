using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using System.Linq;

namespace SOTS.Items.BiomeItems
{
	[AutoloadEquip(EquipType.Body)]
	public class ObsidianMeteorChest : ModItem
	{
		public override void SetDefaults()
		{

			item.width = 30;
			item.height = 20;

			item.value = 100000;
			item.rare = 6;
			item.defense = 6;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Obsidian Meteorite Chestplate");
			Tooltip.SetDefault("Grants a double jump\n15% increased throwing crit chance");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == mod.ItemType("ObsidianMeteorHelmet") && legs.type == mod.ItemType("ObsidianMeteorLeggings");
        }

		public override void UpdateEquip(Player player)
		{
		
			player.thrownCrit += 15;
			player.doubleJumpCloud = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ObsidianScale", 24);
			recipe.AddIngredient(ItemID.MeteoriteBar, 32);
			recipe.AddIngredient(ItemID.Obsidian, 120);
			recipe.AddIngredient(ItemID.HellstoneBar, 6);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}

	}
}