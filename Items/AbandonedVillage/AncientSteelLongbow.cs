using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.AbandonedVillage
{
	public class AncientSteelLongbow : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.damage = 15;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 30;
			Item.height = 64;
			Item.useTime = 44;
			Item.useAnimation = 44;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 4f;
			Item.value = Item.sellPrice(0, 0, 80, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = null;
			Item.autoReuse = false;
			Item.channel = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Evil.AncientSteelLongbow>();
			Item.shootSpeed = 8f;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.useAmmo = AmmoID.Arrow;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Projectiles.Evil.AncientSteelLongbow>(), damage, knockback, player.whoAmI, (int)(Item.useTime / SOTSPlayer.ModPlayer(player).attackSpeedMod), type);
			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<AncientSteelBar>(), 12).AddRecipeGroup(RecipeGroupID.Wood, 20).AddTile(TileID.Anvils).Register();
		}
	}
}