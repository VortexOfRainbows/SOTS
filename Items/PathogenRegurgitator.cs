using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Projectiles.BiomeChest;

namespace SOTS.Items
{
    public class PathogenRegurgitator : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pathogen Regurgitator");
            Tooltip.SetDefault("Fire pathogen balls\nHits may infect enemies for 12 damage per second\nWhen an infected enemy is killed, it releases more pathogen balls");
        }
        public override void SetDefaults()
        {
            item.damage = 40;
            item.ranged = true;
            item.width = 58;
            item.height = 34;
            item.useTime = 24;
            item.useAnimation = 24;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 3;
            item.value = Item.sellPrice(0, 20, 0, 0);
            item.rare = ItemRarityID.Yellow;
            item.UseSound = SoundID.Item34;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<Pathogen>();
            item.shootSpeed = 7.5f;
            if (!Main.dedServ)
            {
                item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/PathogenRegurgitator_Glow");
                item.GetGlobalItem<ItemUseGlow>().glowOffsetX = (int)((Vector2)HoldoutOffset()).X;
                item.GetGlobalItem<ItemUseGlow>().glowOffsetY = (int)((Vector2)HoldoutOffset()).Y;
            }
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 speed = new Vector2(speedX, speedY);
            position += speed.SafeNormalize(Vector2.Zero) * 48;
            int amt = 2;
            if (Main.rand.NextBool(5))
                amt++;
            if (Main.rand.NextBool(7))
                amt++;
            for (int i = 0; i < amt; i++)
            {
                speed = new Vector2(speedX, speedY);
                speed = speed.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-amt * 2, amt)));
                speed.X += Main.rand.NextFloat(-amt, amt) * 0.5f;
                speed.Y += Main.rand.NextFloat(-amt, amt) * 0.5f;
                Projectile.NewProjectile(position, speed, type, damage, knockBack, player.whoAmI);
            }
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, -4);
        }
    }
}
