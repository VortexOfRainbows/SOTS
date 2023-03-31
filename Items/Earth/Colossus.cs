using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using Terraria.DataStructures;
using System;
using SOTS.Projectiles.Blades;

namespace SOTS.Items.Earth
{
	public class Colossus : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.damage = 80;
            Item.DamageType = DamageClass.Melee;  
            Item.width = 74;
            Item.height = 78;  
            Item.useTime = 32; 
            Item.useAnimation = 32;
            Item.useStyle = ItemUseStyleID.Shoot;		
            Item.knockBack = 8f;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = null;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ColossusSlash>(); 
            Item.shootSpeed = 16f;
            Item.noUseGraphic = true; 
            Item.noMelee = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 3 * Math.Sign(velocity.X) * player.gravDir, -1);
			return false;
		}
        public override bool CanPickup(Player player)
        {
            return Item.timeSinceItemSpawned > 40;
        }
    }
}
