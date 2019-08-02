using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SOTS.Items.SoldStuff
{
	[AutoloadEquip(EquipType.Wings)]
	public class Laserpack : ModItem
	{ int direction = 0;
		public override void SetStaticDefaults()
		{	
		DisplayName.SetDefault("Laserpack");
			Tooltip.SetDefault("Boost it up, with lazhars!");
		}

		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 56;
			item.value = 12500000;
			item.rare = 9;
			item.accessory = true;
		}
		//these wings use the same values as the solar wings
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
		
			player.wingTimeMax = 180;
			if(player.gravDir == 1f)
			{
				 if (player.controlJump)
            {
				Projectile.NewProjectile((player.Center.X), player.Center.Y, Main.rand.Next(-2, 3), Main.rand.Next(17, 22), 440, 63, 0, player.whoAmI);
			}

		
		}
		}

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
			ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			ascentWhenFalling = 0.85f;
			ascentWhenRising = 0.5f;
			maxCanAscendMultiplier = 0.135f;
			maxAscentMultiplier = 3f;
			constantAscend = 0.135f;
		}

		public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
		{
			speed = 9f;
			acceleration *= 2.2f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "DoubleMinipack", 2);
			recipe.AddIngredient(ItemID.LaserMachinegun, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}