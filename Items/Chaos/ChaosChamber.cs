using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Items.Pyramid;
using SOTS.Projectiles.Chaos;

namespace SOTS.Items.Chaos
{
	public class ChaosChamber : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chaos Chamber");
			Tooltip.SetDefault("Unloads almost as fast as the trigger is pulled\nBullets gain homing capabilities\nTransforms bullets into laser balls every 6th shot");
		}
		public override void SetDefaults()
		{
            item.damage = 66;   
            item.ranged = true;   
            item.width = 48;    
            item.height = 26;  
            item.useTime = 5;  
            item.useAnimation = 5;
            item.useStyle = ItemUseStyleID.HoldingOut;    
            item.noMelee = true; 
            item.knockBack = 1;
            item.value = Item.sellPrice(0, 12, 0, 0);
            item.rare = ItemRarityID.Yellow;
            item.UseSound = SoundID.Item36;
            item.autoReuse = false;
            item.shoot = ProjectileID.PurificationPowder; 
            item.shootSpeed = 15f;
			item.useAmmo = AmmoID.Bullet;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(2, 0);
		}
		int counter = 0;
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			item.reuseDelay = 0;
			counter++;
			if (counter == 5)
			{
				item.reuseDelay = 4;
			}
			if (counter >= 6)
			{
				counter = 0;
				Main.PlaySound(SoundID.Item38, (int)position.X, (int)position.Y);
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<ChaosBallFriendly>(), damage, knockBack, player.whoAmI);
			}
			else
			{
				Projectile proj = Projectile.NewProjectileDirect(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
				proj.GetGlobalProjectile<SOTSProjectile>().affixID = -3; //this sould sync automatically on the SOTSProjectile end
			}
			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<PhaseBar>(), 15);
			recipe.AddIngredient(ItemID.VenusMagnum, 1);
			recipe.AddIngredient(ModContent.ItemType<GhoulBlaster>(), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
