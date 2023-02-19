using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using Terraria.DataStructures;
using System;
using SOTS.Projectiles.Blades;
using SOTS.Items.Fragments;

namespace SOTS.Items.Invidia
{
	public class VesperaNanDao : VoidItem
	{
		public override void SetStaticDefaults()
        {
           this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 16;
            Item.DamageType = DamageClass.Melee;  
            Item.width = 14;
            Item.height = 54;  
            Item.useTime = 22; 
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Shoot;		
            Item.knockBack = 2.5f;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = null;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<VesperaSlash>(); 
            Item.shootSpeed = 16f;
            Item.noUseGraphic = true; 
            Item.noMelee = true;
        }
        public override int GetVoid(Player player)
        {
            return 4;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 2 * Math.Sign(velocity.X) * player.gravDir, 1);
			return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient<Evostone>(20).AddIngredient(ItemID.StoneBlock, 50).AddIngredient<FragmentOfEarth>(1).AddTile(TileID.Furnaces).Register();
        }
    }
}
