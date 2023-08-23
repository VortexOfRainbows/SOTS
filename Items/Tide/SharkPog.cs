using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Projectiles.Tide;
using Terraria.DataStructures;
using System;

namespace SOTS.Items.Tide
{
	public class SharkPog : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.damage = 8;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 62;
            Item.height = 30;
            Item.useTime = 11; 
            Item.useAnimation = 11;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.noMelee = true;
			Item.knockBack = 0.1f;  
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item85;
            Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<PogBubble>(); 
            Item.shootSpeed = 11.0f;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-8, 0);
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			int lowerBound = Main.rand.Next(2) + Main.rand.Next(2);
            int upperBound = Main.rand.Next(2) + Main.rand.Next(2);
            for (int i = -lowerBound; i <= upperBound; i++)
			{
				Vector2 newVelocity = velocity.RotatedBy(MathHelper.ToRadians(15 * (float)Math.Sqrt(Math.Abs(i)) * Math.Sign(i))) + Main.rand.NextVector2Circular(3, 3);
				Projectile.NewProjectile(source, position + velocity.SafeNormalize(Vector2.Zero) * 32, newVelocity, type, damage, knockback, player.whoAmI);
			}
			return false;
		}
	}
}
