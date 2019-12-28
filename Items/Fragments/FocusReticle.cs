using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework.Graphics;


namespace SOTS.Items.Fragments
{
	public class FocusReticle : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Focus Reticle");
			Tooltip.SetDefault("25% increased crit chance\nCritical strikes deal 50 more damage\nCritical strikes may afflict enemies with frostburn, burn, or cursed flames\nCritical strikes steal life and regenerate void\nImmunity to bleeding and poisoned debuffs");
		}
		public override void SetDefaults()
		{
            item.width = 36;     
            item.height = 34;  
            item.value = Item.sellPrice(0, 15, 25, 0);
            item.rare = 9;
			item.accessory = true;
			item.defense = 1;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			
			modPlayer.CritLifesteal += 1 + (Main.rand.Next(4) == 0 ? 1 : 0);
			modPlayer.CritVoidsteal += 1f;
			modPlayer.CritFrost = true;
			modPlayer.CritFire = true;
			modPlayer.CritCurseFire = true;
			player.meleeCrit += 25;
			player.rangedCrit += 25;
			player.magicCrit += 25;
			player.thrownCrit += 25;
			modPlayer.CritBonusDamage += 25;
            player.buffImmune[BuffID.Bleeding] = true; 
            player.buffImmune[BuffID.Poisoned] = true; 
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "FocusCrystal", 1);
			recipe.AddIngredient(null, "CursedIcosahedron", 1);
			recipe.AddIngredient(null, "EyeOfChaos", 1);
			recipe.AddIngredient(null, "SoulCharm", 1);
			recipe.AddIngredient(ItemID.LunarBar, 12);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
