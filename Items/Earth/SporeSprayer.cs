using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Projectiles;

namespace SOTS.Items.Earth
{
    public class SporeSprayer : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spore Sprayer");
            Tooltip.SetDefault("Fires a burst of Spore Clouds that do 66% damage whenever an arrow is launched");
        }
        public override void SetDefaults()
        {
            Item.damage = 11;
            Item.ranged = true;
            Item.width = 30;
            Item.height = 60;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.HoldingOut;
            Item.noMelee = true;
            Item.knockBack = 0.5f;
            Item.value = Item.sellPrice(0, 0, 40, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 8f;
            Item.useAmmo = AmmoID.Arrow; 
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-1, 0);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            speedX += Main.rand.NextFloat(-1f, 1f);
            speedY += Main.rand.NextFloat(-1f, 1f);
            bool field = false;
            for (int j = -1; j <= 1; j += 2)
                for (int i = 0; i < 3; i++)
                {
                    if(!Main.rand.NextBool(3))
                    {
                        Vector2 burstDirection = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians((7f + 35 * i) * j));
                        Projectile.NewProjectile(position, burstDirection, ModContent.ProjectileType<SporeCloudFriendly>(), (int)(damage * 0.66f), knockBack, player.whoAmI);
                        field = true;
                    }
                }
            if(field)
                Main.PlaySound(2, (int)position.X, (int)position.Y, 34, 0.7f, -0.1f);
            return true; 
        }
    }
}
