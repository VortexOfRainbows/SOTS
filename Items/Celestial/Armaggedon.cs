using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Void;
namespace SOTS.Items.Celestial
{
	public class Armaggedon : VoidItem
	{	float coolDown = 22f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Armaggedon");
			Tooltip.SetDefault("'Power straight from the heavens'\nFirerate increases after each use\nProvides a small light source");
		}
		public override void SafeSetDefaults()
		{
			item.damage = 69;
			item.melee = true;
			item.width = 26;
			item.height = 26;
            item.useTime = 25;
            item.useAnimation = 25;
			item.useStyle = 5;
			item.knockBack = 10.25f;
            item.value = Item.sellPrice(0, 9, 50, 0);
            item.rare = 8;
            item.UseSound = SoundID.Item19;
            item.autoReuse = true;       
			item.shoot = mod.ProjectileType("BluePunch"); 
            item.shootSpeed = 7f;
			item.consumable = false;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.crit = 10;
		}
		int count = 0;
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			coolDown -= 2f;
			int numberProjectiles = 2;
			for (int i = 0; i < numberProjectiles; i++)
			{
				count++;
				type = count % 3 == 0 ? mod.ProjectileType("BluePunch") : count % 3 == 1 ? mod.ProjectileType("PurplePunch") : mod.ProjectileType("SilverPunch");
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(18)); 
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI, 0, Main.rand.Next(2) == 0 ? 1 : -1);
            }
			return false; 
		}
		public override void UpdateInventory(Player player)
		{
			Lighting.AddLight(player.Center, 1.05f, 1.05f, 1.05f);
			coolDown += 0.01f;
			
			if(coolDown > 22f)
				coolDown = 22f;
			
			if(coolDown < 8f)
				coolDown = 8f;
			
			item.useTime = (int)coolDown;
			item.useAnimation = (int)coolDown;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "StarShard", 15);
			recipe.AddIngredient(null, "JeweledGauntlet", 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void GetVoid(Player player)
		{
			voidMana = 3;
		}
	}
}