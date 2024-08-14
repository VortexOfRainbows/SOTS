using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using Terraria.DataStructures;
using System;
using SOTS.Projectiles.Blades;

namespace SOTS.Items.ChestItems
{
	public class BetrayersKnife : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.damage = 25;
            Item.DamageType = DamageClass.Melee;  
            Item.width = 14;
            Item.height = 62;  
            Item.useTime = 24; 
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;		
            Item.knockBack = 2.25f;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = null;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<BetrayersSlash>(); 
            Item.shootSpeed = 16f;
            Item.noUseGraphic = true; 
            Item.noMelee = true;
            Item.shopCustomPrice = Item.buyPrice(1, 0, 0, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 2 * SOTSUtils.SignNoZero(velocity.X) * player.gravDir, 1);
			return false;
		}
    }
}
