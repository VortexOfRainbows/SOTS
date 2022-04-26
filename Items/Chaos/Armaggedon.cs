using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Void;
using SOTS.Items.Pyramid;
using SOTS.Projectiles.Celestial;
using SOTS.Projectiles.Chaos;

namespace SOTS.Items.Chaos
{
	public class Armaggedon : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Armaggedon");
			Tooltip.SetDefault("Unleash a flurry punches\nProvides a small light source\n'Power from the heavens'");
		}
		public override void SafeSetDefaults()
		{
			item.damage = 270;
			item.melee = true;
			item.width = 28;
			item.height = 26;
            item.useTime = 10;
            item.useAnimation = 30;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 8.0f;
            item.value = Item.sellPrice(0, 14, 0, 0);
            item.rare = ItemRarityID.Yellow;
            item.UseSound = SoundID.Item19;
            item.autoReuse = true;       
			item.shoot = ModContent.ProjectileType<ChaosPunch>(); 
            item.shootSpeed = 11f;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.crit = 10;
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-15, 15)));
			Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			return false; 
		}
		public override void UpdateInventory(Player player)
		{
			Lighting.AddLight(player.Center, 1.00f, 1.00f, 1.00f);
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
			return 6;
		}
	}
}