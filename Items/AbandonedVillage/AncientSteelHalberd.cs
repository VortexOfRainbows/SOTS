using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.AbandonedVillage
{
	public class AncientSteelHalberd : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.damage = 6;
			Item.DamageType = DamageClass.Melee;
			Item.width = 66;
			Item.height = 66;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 1.6f;
            Item.value = Item.sellPrice(0, 0, 60, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.DD2_GhastlyGlaivePierce;
			Item.autoReuse = true;            
			Item.shoot = ModContent.ProjectileType<Projectiles.Evil.AncientSteelHalberd>(); 
            Item.shootSpeed = 6.2f;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			//Item.channel = true;
		}
        public override void ModifyWeaponCrit(Player player, ref float crit)
        {
			crit = 0;
        }
        public override bool CanUseItem(Player player)
        {
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<AncientSteelBar>(), 16).AddRecipeGroup(RecipeGroupID.Wood, 20).AddTile(TileID.Anvils).Register();
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity.SafeNormalize(Vector2.Zero) * 6.2f, type, damage, knockback, player.whoAmI, SOTSPlayer.ApplyAttackSpeedClassModWithGeneric(player, Item.DamageType, Item.useTime));
			return false; 
		}
    }
}
	
