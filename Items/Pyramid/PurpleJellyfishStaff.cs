using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Void;

namespace SOTS.Items.Pyramid
{
	public class PurpleJellyfishStaff : VoidItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Purple Jellyfish Staff");
			Tooltip.SetDefault("Fires 2 purple orbs that, upon detonation, release purple thunder towards your cursor\nPurple thunder chains off enemies for 70% damage\nProvides a light source while in the inventory");
		}
		public override void SafeSetDefaults()
		{
            item.damage = 32;
			item.magic = true;
            item.width = 36;    
            item.height = 36; 
            item.useTime = 20; 
            item.useAnimation = 20;
            item.useStyle = 5;    
            item.knockBack = 3.5f;
			item.value = Item.sellPrice(0, 2, 25, 0);
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
			voidMana = 7;
		}
		public override void UpdateInventory(Player player)
		{ 
			Lighting.AddLight(player.Center, 0.75f, 0.75f, 0.75f);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PinkJellyfishStaff", 1);
			recipe.AddIngredient(null, "BlueJellyfishStaff", 1);
			recipe.AddIngredient(null, "DissolvingAether", 1);
			recipe.AddIngredient(null, "CursedMatter", 5);
			recipe.AddIngredient(ItemID.SoulofNight, 12);
			recipe.AddIngredient(ItemID.Amethyst, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(90)); 
            Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI, 0);
            Projectile.NewProjectile(position.X, position.Y, -perturbedSpeed.X, -perturbedSpeed.Y, type, damage, knockBack, player.whoAmI, 0);
            return false;
		}
	}
}
