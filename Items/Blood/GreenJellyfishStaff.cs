using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Void;

namespace SOTS.Items.Blood
{
	public class GreenJellyfishStaff : VoidItem
	{	int timer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Green Jellyfish Staff");
			Tooltip.SetDefault("Fires 2 green orbs that, upon detonation, release green thunder towards your cursor\nDecreases void regen by 0.75 while in the inventory, but also provides a light source\nThe thunder will not hurt players");
		}
		public override void SafeSetDefaults()
		{
            item.damage = 60;
			item.magic = true;
            item.width = 32;    
            item.height = 32; 
            item.useTime = 18; 
            item.useAnimation = 18;
            item.useStyle = 5;    
            item.knockBack = 3.5f;
			item.value = Item.sellPrice(0, 7, 50, 0);
            item.rare = 7;
			item.UseSound = SoundID.Item92;
            item.noMelee = true; 
            item.autoReuse = true;
            item.shootSpeed = 6.25f; 
			item.shoot = mod.ProjectileType("GreenThunderCluster");
			Item.staff[item.type] = true; 

		}
		public override void GetVoid(Player player)
		{
				voidMana = 5;
		}
		public override void UpdateInventory(Player player)
		{
				Lighting.AddLight(player.Center, 7.25f, 7.25f, 7.25f);
				VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
				SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
				//timer++;
				voidPlayer.voidRegen -= 0.075f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PurpleJellyfishStaff", 1);
			recipe.AddIngredient(ItemID.SoulofLight, 15);
			recipe.AddIngredient(ItemID.SoulofSight, 15);
			recipe.AddIngredient(null, "BloodEssence", 25);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(180)); 
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				  
				  
                  Vector2 perturbedSpeed2 = perturbedSpeed.RotatedBy(MathHelper.ToRadians(180)); 
				  
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed2.X, perturbedSpeed2.Y, type, damage, knockBack, player.whoAmI);
				  
				  
              
                  //Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
            return false;
		}
	}
}
