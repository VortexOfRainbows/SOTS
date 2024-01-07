using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using SOTS.Buffs;
using SOTS.Items.Fragments;
using SOTS.Projectiles;
using SOTS.Projectiles.Evil;
using SOTS.Void;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.AbandonedVillage
{
	public class MineralSpewer : VoidItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
        }
        public override void SafeSetDefaults()
        {
            Item.damage = 9; 
            Item.DamageType = DamageClass.Ranged;  
            Item.width = 58;   
            Item.height = 18;
            Item.useTime = 20; 
            Item.useAnimation = 20;
            Item.reuseDelay = 10;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item61;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<SootBall>();
            Item.channel = true;
            Item.shootSpeed = 10f;
        }
        public override int GetVoid(Player player)
        {
            return 5;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-20f, 0);
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 offset = -HoldoutOffset().Value;
            offset.Y *= -Math.Sign(velocity.X);
            position += offset.RotatedBy(velocity.ToRotation());
            position += velocity.SafeNormalize(Vector2.Zero) * 12f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for(int i = 0; i < 5; i++)
            {
                Vector2 circularRand = Main.rand.NextVector2Circular(3, 3);
                Projectile.NewProjectile(source, position, velocity * Main.rand.NextFloat(0.2f + i / 5f, 1.2f) + circularRand, type, damage, knockback, player.whoAmI, 0, Main.rand.NextFloat(0.4f, 1f));
            }
            return false;
        }
    }
}
