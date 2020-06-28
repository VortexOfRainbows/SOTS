using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using SOTS.Void;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class AbyssalFury : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Abyssal Fury");
			Tooltip.SetDefault("");
		}
		public override void SafeSetDefaults()
		{
			item.damage = 57;
			item.melee = true;
			item.width = 48;
			item.height = 48;
			item.useTime = 11;
			item.useAnimation = 33;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 5f;
			item.value = Item.sellPrice(0, 20, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;            
			item.shoot = mod.ProjectileType("LightspeedBlade"); 
            item.shootSpeed = 7f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "VibrantBlade", 1);
			recipe.AddIngredient(null, "ObsidianEruption", 1);
			recipe.AddIngredient(ItemID.Starfury, 1);
			recipe.AddIngredient(ItemID.EnchantedSword, 1);
			recipe.AddIngredient(null, "BrokenVillainSword", 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void GetVoid(Player player)
		{
			voidMana = 12;
		}
		int rotate = 0;
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			int numberProjectiles = 1;  //This defines how many projectiles to shot
            for (int index = 0; index < numberProjectiles; ++index)
			{
				rotate += Main.rand.Next(45);
				Vector2 spawnPos = new Vector2((float)(player.Center.X + (200f * -player.direction * Main.rand.Next(8,13)) + (Main.mouseX + Main.screenPosition.X - player.position.X)), player.Center.Y - 26f * Main.rand.Next(8, 13));
				spawnPos.Y += 0.5f * (player.Center.Y - Main.MouseWorld.Y);
				Vector2 circularPos = new Vector2(Main.rand.Next(64, 196 + 1), 0).RotatedBy(MathHelper.ToRadians(rotate));
				spawnPos += circularPos;
				Vector2 speed = Main.MouseWorld - spawnPos;
				speed.Normalize();
				speed *= (float)Math.Sqrt(speedX * speedX + speedY * speedY);

				Projectile.NewProjectile(spawnPos.X, spawnPos.Y, speed.X, speed.Y, type, damage, knockBack, Main.myPlayer, 0.0f, Main.MouseWorld.X);
            }
            return false;
		}
	}
}