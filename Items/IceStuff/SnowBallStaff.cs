using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.IceStuff
{
	public class SnowBallStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snowball Staff");
			Tooltip.SetDefault("Now you can be the weather forecaster!");
		}
		public override void SetDefaults()
		{

			item.damage = 40;
			item.magic = true;
			item.width = 62;
			item.height = 62;
			item.useTime = 12;
			item.useAnimation = 12;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 120000;
			item.rare = 6;
			item.UseSound = SoundID.Item24;
			item.autoReuse = true;            
			item.shoot = 166; 
            item.shootSpeed = 14;
			item.mana = 2;
			Item.staff[item.type] = true; 


		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "AbsoluteBar", 16);
			recipe.AddIngredient(ItemID.FrostStaff, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int numberProjectiles = 1;  //This defines how many projectiles to shot
            for (int index = 0; index < numberProjectiles; ++index)
            {
                Vector2 vector2_1 = new Vector2((float)((double)player.position.X + (double)player.width * 0.5 + (double)(-player.direction) + ((double)Main.mouseX + (double)Main.screenPosition.X - (double)player.position.X)), (float)((double)player.position.Y + (double)player.height * 0.5 - 600.0));   //this defines the projectile width, direction and position
                vector2_1.X = (float)(((double)vector2_1.X + (double)player.Center.X) / 2.0);
                vector2_1.Y -= (float)(100 * index);
                float num12 = (float)Main.mouseX + Main.screenPosition.X - vector2_1.X;
                float num13 = (float)Main.mouseY + Main.screenPosition.Y - vector2_1.Y;
                if ((double)num13 < 0.0) num13 *= -1f;
                if ((double)num13 < 20.0) num13 = 20f;
                float num14 = (float)Math.Sqrt((double)num12 * (double)num12 + (double)num13 * (double)num13);
                float num15 = item.shootSpeed / num14;
                float num16 = num12 * num15;
                float num17 = num13 * num15;
                float SpeedX = num16 * 1.4f;
                float SpeedY = num17 * 1.4f; 
                Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, type, damage, knockBack, Main.myPlayer, 0.0f, 2);
                Projectile.NewProjectile(vector2_1.X + 20, vector2_1.Y, SpeedX, SpeedY, type, damage, knockBack, Main.myPlayer, 0.0f, 2);
                Projectile.NewProjectile(vector2_1.X - 20, vector2_1.Y, SpeedX, SpeedY, type, damage, knockBack, Main.myPlayer, 0.0f, 2);
                Projectile.NewProjectile(vector2_1.X + 40, vector2_1.Y, SpeedX, SpeedY, type, damage, knockBack, Main.myPlayer, 0.0f, 2);
                Projectile.NewProjectile(vector2_1.X - 40, vector2_1.Y, SpeedX, SpeedY, type, damage, knockBack, Main.myPlayer, 0.0f, 2);
            }
            return false;
	}
	}
}