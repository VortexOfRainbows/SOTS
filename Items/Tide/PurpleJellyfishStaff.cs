using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Void;
using SOTS.Projectiles.Lightning;
using SOTS.Items.Pyramid;
using SOTS.Items.Fragments;
using Terraria.DataStructures;

namespace SOTS.Items.Tide
{
	public class PurpleJellyfishStaff : VoidItem
	{	
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 32;
			Item.DamageType = DamageClass.Magic;
            Item.width = 36;    
            Item.height = 36; 
            Item.useTime = 20; 
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 3.5f;
			Item.value = Item.sellPrice(0, 2, 25, 0);
            Item.rare = ItemRarityID.LightPurple;
			Item.UseSound = SoundID.Item92;
            Item.noMelee = true; 
            Item.autoReuse = false;
            Item.shootSpeed = 6.25f; 
			Item.shoot = ModContent.ProjectileType<PurpleThunderCluster>();
			Item.staff[Item.type] = true; 
		}
		public override int GetVoid(Player player)
		{
			return  7;
		}
		public override void UpdateInventory(Player player)
		{ 
			Lighting.AddLight(player.Center, 0.75f, 0.75f, 0.75f);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<PinkJellyfishStaff>(), 1).AddIngredient(ModContent.ItemType<BlueJellyfishStaff>(), 1).AddIngredient(ModContent.ItemType<DissolvingAether>(), 1).AddIngredient(ModContent.ItemType<CursedMatter>(), 5).AddIngredient(ItemID.SoulofNight, 12).AddIngredient(ItemID.Amethyst, 1).AddTile(TileID.MythrilAnvil).Register();
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(90)); 
            Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI, 0);
            Projectile.NewProjectile(source, position.X, position.Y, -perturbedSpeed.X, -perturbedSpeed.Y, type, damage, knockback, player.whoAmI, 0);
            return false;
		}
	}
}
