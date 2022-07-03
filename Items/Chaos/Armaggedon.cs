using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Void;
using SOTS.Items.Pyramid;
using SOTS.Projectiles.Celestial;
using SOTS.Projectiles.Chaos;
using Terraria.DataStructures;

namespace SOTS.Items.Chaos
{
	public class Armaggedon : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Armageddon");
			Tooltip.SetDefault("Unleash a flurry punches\nProvides a small light source\n'Power from the heavens'");
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
			Item.damage = 270;
			Item.DamageType = DamageClass.Melee;
			Item.width = 28;
			Item.height = 26;
            Item.useTime = 10;
            Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 8.0f;
            Item.value = Item.sellPrice(0, 14, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item19;
            Item.autoReuse = true;       
			Item.shoot = ModContent.ProjectileType<ChaosPunch>(); 
            Item.shootSpeed = 11f;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.crit = 10;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-15, 15)));
			Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
			return false; 
		}
		public override void UpdateInventory(Player player)
		{
			Lighting.AddLight(player.Center, 1.00f, 1.00f, 1.00f);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<JeweledGauntlet>(), 1).AddIngredient(ModContent.ItemType<PhaseBar>(), 20).AddTile(TileID.MythrilAnvil).Register();
		}
		public override int GetVoid(Player player)
		{
			return 6;
		}
	}
}