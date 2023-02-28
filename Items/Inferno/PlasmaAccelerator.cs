using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SOTS.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Projectiles.Inferno;
using SOTS.Items.Fragments;
using Terraria.DataStructures;

namespace SOTS.Items.Inferno
{
	public class PlasmaAccelerator : ModItem
	{
		public override void SetStaticDefaults()
		{
            this.SetResearchCost(1);
        }
		public override void SetDefaults()
		{
            Item.damage = 44; 
            Item.DamageType = DamageClass.Ranged;  
            Item.width = 28;   
            Item.height = 64; 
            Item.useTime = 10; 
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(0, 7, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item91;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<PlasmaphobiaBolt>(); 
            Item.shootSpeed = 12.5f;
			Item.useAmmo = ItemID.WoodenArrow;
		}
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-1, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position += velocity.SafeNormalize(Vector2.Zero) * 24;
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<PlasmaphobiaBolt>(), damage, knockback, player.whoAmI);
			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Sharanga>(), 1).AddIngredient(ModContent.ItemType<DissolvingAether>(), 1).AddIngredient(ModContent.ItemType<DissolvingNether>(), 1).AddIngredient(ItemID.Ectoplasm, 5).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}
