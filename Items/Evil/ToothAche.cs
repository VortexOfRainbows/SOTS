using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using Terraria.DataStructures;
using System;
using SOTS.Projectiles.Blades;

namespace SOTS.Items.Evil
{
	public class ToothAche : VoidItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 27;
            Item.DamageType = DamageClass.Melee;  
            Item.width = 84;
            Item.height = 64;  
            Item.useTime = 30; 
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;		
            Item.knockBack = 3.6f;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = null;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ToothAcheSlash>(); 
            Item.shootSpeed = 16f;
            Item.noUseGraphic = true; 
            Item.noMelee = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 4 * Math.Sign(velocity.X) * player.gravDir, Main.rand.NextFloat(0.98f, 1.02f));
			return false;
		}
        public override int GetVoid(Player player)
        {
            return 9;
        }
    }
}
