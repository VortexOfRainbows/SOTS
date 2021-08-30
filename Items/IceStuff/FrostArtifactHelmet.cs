using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.IceStuff
{
	[AutoloadEquip(EquipType.Head)]
	public class FrostArtifactHelmet : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 22;
            item.value = Item.sellPrice(0, 6, 25, 0);
			item.rare = ItemRarityID.Lime;
			item.defense = 16;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frost Artifact Helmet");
			Tooltip.SetDefault("14% increased melee and ranged damage\nA Frost Storm surrounds you, frostburning nearby enemies");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("FrostArtifactChestplate") && legs.type == mod.ItemType("FrostArtifactTrousers");
        }
        public override void UpdateArmorSet(Player player)
        {	
			player.setBonus = "Nearby enemies and projectiles will have their velocities slowed";
		}
		public override void UpdateEquip(Player player)
		{
			player.meleeDamage += 0.14f;
			player.rangedDamage += 0.14f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FrostHelmet, 1);
			recipe.AddIngredient(null, "AbsoluteBar", 16);
			recipe.SetResult(this);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddRecipe();
		}
	}
}