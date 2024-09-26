using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using SOTS.Void;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Invidia
{
	public class VesperaLongbow : VoidItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
			Item.damage = 15;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 26;
			Item.height = 54;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 4f;
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = null;
			Item.autoReuse = false;
			Item.channel = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Earth.EvostoneLongbow>();
			Item.shootSpeed = 8f;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.useAmmo = AmmoID.Arrow;
		}
        public override int GetVoid(Player player)
        {
            return 10;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Projectiles.Earth.EvostoneLongbow>(), damage, knockback, player.whoAmI, SOTSPlayer.ApplyAttackSpeedClassModWithGeneric(player, Item.DamageType, Item.useTime), type);
			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<Evostone>(20).AddIngredient(ItemID.StoneBlock, 50).AddIngredient<FragmentOfEarth>(1).AddTile(TileID.Furnaces).Register();
		}
	}
}