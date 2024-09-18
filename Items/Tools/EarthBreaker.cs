using Microsoft.Xna.Framework;
using SOTS.Dusts;
using SOTS.Projectiles.Earth;
using SOTS.Void;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Tools
{
	public class EarthBreaker : VoidItem
	{	
		public override void SetStaticDefaults()
		{
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 15;
            Item.DamageType = DamageClass.Melee;  
            Item.width = 44;   
            Item.height = 44;   
            Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
            Item.useTime = 15;
            Item.useAnimation = 30;
			Item.pick = 59;
			Item.knockBack = 2f;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.tileBoost = 3;
			Item.autoReuse = true;
			Item.consumable = false;

            Item.shoot = ModContent.ProjectileType<EarthBreakerPickaxe>();
            Item.shootSpeed = 12;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(hitbox.TopLeft(), hitbox.Width, hitbox.Height, ModContent.DustType<PixelDust>(), player.direction * 2, 0f);
                dust.velocity *= 0.3f;
                dust.scale = 1f;
                dust.fadeIn = 10f;
                dust.color = ColorHelpers.EarthColor;
                dust.color.A = 0;
            }
        }
        public override bool BeforeUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.noMelee = true;
                Item.noUseGraphic = true;
                Item.channel = false;
            }
            else
            {
                Item.noMelee = false;
                Item.noUseGraphic = false;
                Item.channel = true;
            }
            return true;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 2 * SOTSUtils.SignNoZero(velocity.X) * player.gravDir, 1);
            return false;
        }
        public override int GetVoid(Player player)
        {
            return 10;
        }
        public override bool BeforeDrainVoid(Player player)
        {
            return player.altFunctionUse == 2;
        }
    }
}
