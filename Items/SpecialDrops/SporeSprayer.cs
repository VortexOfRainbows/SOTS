using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SOTS.Items.SpecialDrops
{
    public class SporeSprayer : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spore Sprayer");
            Tooltip.SetDefault("Converts wooden arrows into spore arrows");
        }
        public override void SetDefaults()
        {
            item.damage = 5;
            item.ranged = true;
            item.width = 30;
            item.height = 60;
            item.useTime = 13;
            item.useAnimation = 13;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 1f;
            item.value = Item.sellPrice(0, 0, 40, 0);
            item.rare = 1;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shoot = ProjectileID.WoodenArrowFriendly;
            item.shootSpeed = 7.75f;
            item.useAmmo = AmmoID.Arrow; 
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-1, 0);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            speedX += Main.rand.Next(-30, 31) * 0.025f;
            speedY += Main.rand.Next(-30, 31) * 0.025f;
            if (type == ProjectileID.WoodenArrowFriendly) 
            {
                type = mod.ProjectileType("SporeArrow"); 
            }
            return true; 
        }
    }
}
