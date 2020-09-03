using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Celestial
{
	public class DimensionShredder : ModItem
	{	int index1 = -1;
		float rot = 0;
		bool inInventory = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dimension Shredder");
			Tooltip.SetDefault("66% chance to not consume ammo\n'Tear a rift through your enemies'");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(1929); //chaingun
			item.damage = 34;
            item.width = 50;   
            item.height = 28;   
			item.rare = 8;
            item.value = Item.sellPrice(0, 8, 25, 0);
            item.shootSpeed = 15.5f;
		}
        public override bool CanUseItem(Player player)
		{
			if(inInventory)
				return true;
			return false;
		}
		public override void HoldItem(Player player) 
		{
			if(index1 == -1)
			{
				index1 = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, mod.ProjectileType("DimensionPortal"), (int)(item.damage * (player.rangedDamage + player.allDamage - 1f)), item.knockBack, player.whoAmI);
			}
			if(index1 != -1)
			{
				Projectile proj = Main.projectile[index1];
				if(proj.type == mod.ProjectileType("DimensionPortal") && proj.active)
				{
					Vector2 rotatePos = new Vector2(96, 0).RotatedBy(MathHelper.ToRadians(rot));
					proj.position.X = rotatePos.X + player.Center.X - proj.width/2;
					proj.position.Y = rotatePos.Y + player.Center.Y - proj.height/2;
					proj.timeLeft = 2;
				}
				else
				{
					index1 = -1;
				}
			}
		}
		public override void UpdateInventory(Player player)
		{
			inInventory = true;
			rot += 1.33f;
		}
		public override bool ConsumeAmmo(Player p)
		{
			if(Main.rand.Next(3) >= 1)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(4, 0);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SanguiteBar", 15);
			recipe.AddIngredient(ItemID.ChainGun, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		int num = 0;
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			num++;
			if(num % 3 == 0)
			{
				Projectile.NewProjectile(position.X + (speedY * 0.2f), position.Y - (speedX * 0.2f), speedX, speedY, type, damage, knockBack, player.whoAmI);
				return false;
			}
			if(num % 3 == 1)
			{
				Projectile.NewProjectile(position.X - (speedY * 0.2f), position.Y + (speedX * 0.2f), speedX, speedY, type, damage, knockBack, player.whoAmI);
				return false;
			}
			inInventory = false;
			return true; 
		}
	}
}
