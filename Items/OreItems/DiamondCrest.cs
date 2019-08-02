using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using System.Linq;

namespace SOTS.Items.OreItems
{
	[AutoloadEquip(EquipType.Body)]
	public class DiamondCrest : ModItem
	{
		public override void SetDefaults()
		{

			item.width = 30;
			item.height = 20;

			item.value = 525000;
			item.rare = 6;
			item.defense = 11;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Diamond Crest");
			Tooltip.SetDefault("15% increase to all damage\nGrants an extra minion slot\nAlso decreases damage taken by 8%");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == mod.ItemType("DiamondSpacer") && legs.type == mod.ItemType("DiamondBoots");
        }

		public override void UpdateEquip(Player player)
		{
		
			player.minionDamage += 0.15f;
			player.meleeDamage += 0.15f;
			player.magicDamage += 0.15f;
			player.rangedDamage += 0.15f;
			player.thrownDamage += 0.15f;
			player.endurance += 0.08f;
			player.maxMinions += 1;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SteelBar", 24);
			recipe.AddIngredient(ItemID.Diamond, 12);
			recipe.AddIngredient(ItemID.Ruby, 12);
			recipe.AddIngredient(ItemID.HellstoneBar, 12);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}

	}
}