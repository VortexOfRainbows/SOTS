using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.SpecialDrops
{
	public class CrystalSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Sword");
			Tooltip.SetDefault("Killed enemies explode into crystal shards");
		}
		public override void SetDefaults()
		{
            item.damage = 60;  //gun damage
            item.melee = true;   //its a gun so set this to true
            item.width = 58;     //gun image width
            item.height = 58;   //gun image  height
            item.useTime = 24;  //how fast 
            item.useAnimation = 24;
            item.useStyle = 1;    
            item.knockBack = 6.5f;
            item.value = 150000;
            item.rare = 6;
            item.UseSound = SoundID.Item1;
            item.autoReuse = false;

		}	
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(502, 20);
			recipe.AddIngredient(ItemID.SoulofLight, 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit) 
		{
			for(int i = 0; i < 5; i++)
			{
			int num1 = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, 58);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			}
			if(damage - (target.defense * 0.5f) > target.life)
			{
				for(int i = 0; i < 24; i++)
				{
					Projectile.NewProjectile(target.Center.X, target.Center.Y, Main.rand.Next(-5,6), Main.rand.Next(-5,6), 90, (int)(60 * player.meleeDamage), 0, Main.myPlayer);
					Projectile.NewProjectile(target.Center.X, target.Center.Y, Main.rand.Next(-3,4), Main.rand.Next(-3,4), 90, (int)(60 * player.meleeDamage), 0, Main.myPlayer);
					Projectile.NewProjectile(target.Center.X, target.Center.Y, Main.rand.Next(-7,8), Main.rand.Next(-7,8), 90, (int)(60 * player.meleeDamage), 0, Main.myPlayer);
				}
			}
		}
	}
}
