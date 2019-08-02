using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops
{
	[AutoloadEquip(EquipType.Wings)]
	public class EonBadge : ModItem
	{ int Probe = -1;
	int Probe2 = -1;
	int Probe3 = -1;
		public override void SetStaticDefaults()
		{	
		DisplayName.SetDefault("Eon Badge");
			Tooltip.SetDefault("Grants infinite jet flight\nSummons Latias to protect you by sapping enemy health\nSummons Latios to protect you with Draco Meteors");
		}

		public override void SetDefaults()
		{
			item.width = 78;
			item.height = 56;
			item.value = 25000000;
			item.rare = 11;
			item.expert = true;
			item.accessory = true;
		}
		//these wings use the same values as the solar wings
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.buffImmune[83] = true;
			player.wingTimeMax += 20000;
			if (Probe == -1)
			{
					Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("Latios"), 120, 0, player.whoAmI);
					}
				if (!Main.projectile[Probe].active || Main.projectile[Probe].type != mod.ProjectileType("Latios"))
				{
					Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("Latios"), 120, 0, player.whoAmI);
				}
				Main.projectile[Probe].timeLeft = 6;
			
			if (Probe2 == -1)
			{
					Probe2 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("Latias"), 21, 0, player.whoAmI);
					}
				if (!Main.projectile[Probe2].active || Main.projectile[Probe2].type != mod.ProjectileType("Latias"))
				{
					Probe2 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("Latias"), 21, 0, player.whoAmI);
				}
				Main.projectile[Probe2].timeLeft = 6;
			
			

		
		}
		

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
			ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			ascentWhenFalling = 0.75f;
			ascentWhenRising = 0.75f;
			maxCanAscendMultiplier = 0.155f;
			maxAscentMultiplier = 7f;
			constantAscend = 0.155f;
		}

		public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
		{
			speed = 24f;
			acceleration *= 6.2f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"BlueDew", 1);
			recipe.AddIngredient(null,"RedDew", 1);
			recipe.AddIngredient(null,"TheHardCore", 1);
			recipe.AddIngredient(ItemID.SoulofFlight, 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}