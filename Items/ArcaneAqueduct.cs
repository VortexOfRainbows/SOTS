using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;


namespace SOTS.Items
{
	public class ArcaneAqueduct : ModItem
	{	int Probe = -1;
		int Probe2 = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arcane Aqueduct");
			Tooltip.SetDefault("Surrounds you with 2 orbital projectiles");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(8, 8));
		}
		public override void SetDefaults()
		{
	
			item.damage = 21;
			item.magic = true;
            item.width = 34;     
            item.height = 34;   
            item.value = Item.sellPrice(0, 0, 75, 0);
            item.rare = 3;
			item.accessory = true;

		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.WaterBolt, 1);
			recipe.AddIngredient(ItemID.AquaScepter, 1);
			recipe.AddIngredient(3066, 25); //smooth marble
			recipe.AddIngredient(null, "SoulResidue", 15);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		float rotation = 0;
		float rotation2 = 0;
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			rotation += 4f;
			rotation2 += 1f;
				if (Probe == -1)
				{
					Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, 27, (int)(item.damage * (1f + (player.magicDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI); //waterbolt proj
				}
				if (!Main.projectile[Probe].active || Main.projectile[Probe].type != 27)
				{
					Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, 27, (int)(item.damage * (1f + (player.magicDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI);
				}
				Main.projectile[Probe].timeLeft = 6;
				if (Probe != -1)
				{
					Projectile proj = Main.projectile[Probe];
					proj.tileCollide = false;
					proj.penetrate = -1;
					Vector2 initialLoop = new Vector2(128, 0).RotatedBy(MathHelper.ToRadians(rotation));
					initialLoop.X /= 2.0f;
					Vector2 properLoop = new Vector2(initialLoop.X, initialLoop.Y).RotatedBy(MathHelper.ToRadians(rotation2));
					proj.position.X = properLoop.X + player.Center.X - proj.width/2;
					proj.position.Y = properLoop.Y + player.Center.Y - proj.height/2;
					//Projectile.NewProjectile(proj.Center.X, proj.Center.Y, 0, 0, 14, item.damage, 0, player.whoAmI); //this was for testing shape
				}
				
				if (Probe2 == -1)
				{
					Probe2 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, 27, (int)(item.damage * (1f + (player.magicDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI);
				}
				if (!Main.projectile[Probe2].active || Main.projectile[Probe2].type != 27)
				{
					Probe2 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, 27, (int)(item.damage * (1f + (player.magicDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI);
				}
				Main.projectile[Probe2].timeLeft = 6;
				if (Probe2 != -1)
				{
					Projectile proj = Main.projectile[Probe2];
					proj.tileCollide = false;
					proj.penetrate = -1;
					Vector2 initialLoop = new Vector2(-128, 0).RotatedBy(MathHelper.ToRadians(rotation));
					initialLoop.Y /= 2.0f;
					Vector2 properLoop = new Vector2(initialLoop.X, initialLoop.Y).RotatedBy(MathHelper.ToRadians(rotation2));
					proj.position.X = properLoop.X + player.Center.X - proj.width/2;
					proj.position.Y = properLoop.Y + player.Center.Y - proj.height/2;
					//Projectile.NewProjectile(proj.Center.X, proj.Center.Y, 0, 0, 14, item.damage, 0, player.whoAmI);
				}
		}
	}
}