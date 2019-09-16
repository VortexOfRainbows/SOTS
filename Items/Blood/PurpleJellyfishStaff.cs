using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Void;

namespace SOTS.Items.Blood
{
	public class PurpleJellyfishStaff : VoidItem
	{	int timer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Purple Jellyfish Staff");
			Tooltip.SetDefault("Fires 2 purple orbs that, upon detonation, release purple thunder towards your cursor\nDrains void slowly while in the inventory, but provides a light source");
		}
		public override void SafeSetDefaults()
		{
            item.damage = 40;
			item.magic = true;
            item.width = 32;    
            item.height = 32; 
            item.useTime = 18; 
            item.useAnimation = 18;
            item.useStyle = 5;    
            item.knockBack = 3.5f;
            item.value = 150000;
            item.rare = 6;
			item.UseSound = SoundID.Item92;
            item.noMelee = true; 
            item.autoReuse = false;
            item.shootSpeed = 6.25f; 
			item.shoot = mod.ProjectileType("PurpleThunderCluster");
			Item.staff[item.type] = true; 

		}
		public override void GetVoid(Player player)
		{
				voidMana = 6;
		}
		public override void UpdateInventory(Player player)
		{
				Lighting.AddLight(player.Center, 14.5f, 0.25f, 11.5f);
				VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
				SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
				//timer++;
				if(modPlayer.bloodDecay)
				{
				timer ++;
				}
				if(timer >= 1800)
				{
					timer = 0;
					voidPlayer.voidMeter -= 1;
				}
			
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PinkJellyfishStaff", 1);
			recipe.AddIngredient(null, "BlueJellyfishStaff", 1);
			recipe.AddIngredient(ItemID.JellyfishNecklace, 1);
			recipe.AddIngredient(null, "RedPowerChamber", 1);
			recipe.AddIngredient(null, "BluePowerChamber", 1);
			recipe.AddIngredient(null, "CursedMatter", 5);
			recipe.AddIngredient(null, "BloodEssence", 5);
			recipe.AddIngredient(ItemID.SoulofNight, 15);
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(90)); 
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
                  Projectile.NewProjectile(position.X, position.Y, -perturbedSpeed.X, -perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              
                  //Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
            return false;
		}
	}
}
