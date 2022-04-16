using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Void;
using SOTS.Items.Pyramid;
using SOTS.Projectiles.Celestial;

namespace SOTS.Items.Chaos
{
	public class Armaggedon : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Armaggedon");
			Tooltip.SetDefault("Provides a small light source\n'Power from the heavens'");
		}
		public override void SafeSetDefaults()
		{
			item.damage = 80;
			item.melee = true;
			item.width = 28;
			item.height = 26;
            item.useTime = 12;
            item.useAnimation = 12;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 10.25f;
            item.value = Item.sellPrice(0, 14, 0, 0);
            item.rare = ItemRarityID.Yellow;
            item.UseSound = SoundID.Item19;
            item.autoReuse = true;       
			item.shoot = ModContent.ProjectileType<BluePunch>(); 
            item.shootSpeed = 9f;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.crit = 10;
		}
		int count = 0;
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			int numberProjectiles = 2;
			for (int i = 0; i < numberProjectiles; i++)
			{
				count++;
				type = count % 3 == 0 ? ModContent.ProjectileType<BluePunch>() : count % 3 == 1 ? ModContent.ProjectileType<PurplePunch>() : ModContent.ProjectileType<SilverPunch>();
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(18)); 
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI, 0, Main.rand.Next(2) == 0 ? 1 : -1);
            }
			return false; 
		}
		public override void UpdateInventory(Player player)
		{
			Lighting.AddLight(player.Center, 1.05f, 1.05f, 1.05f);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<JeweledGauntlet>(), 1);
			recipe.AddIngredient(ModContent.ItemType<PhaseBar>(), 20);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override int GetVoid(Player player)
		{
			return 5;
		}
	}
}