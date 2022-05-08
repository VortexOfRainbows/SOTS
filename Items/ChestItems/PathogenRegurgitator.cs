using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Projectiles.BiomeChest;

namespace SOTS.Items.ChestItems
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
            Item.damage = 45;
            Item.ranged = true;
            Item.width = 58;
            Item.height = 34;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3;
            Item.value = Item.sellPrice(0, 20, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item34;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Pathogen>();
            Item.shootSpeed = 15.5f;
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/ChestItems/PathogenRegurgitator_Glow").Value;
                Item.GetGlobalItem<ItemUseGlow>().glowOffsetX = (int)((Vector2)HoldoutOffset()).X;
                Item.GetGlobalItem<ItemUseGlow>().glowOffsetY = (int)((Vector2)HoldoutOffset()).Y;
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
                speed.X += Main.rand.NextFloat(-amt, amt) * 0.33f;
                speed.Y += Main.rand.NextFloat(-amt, amt) * 0.33f;
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
