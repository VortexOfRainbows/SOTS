using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Items.BaseWeapons;
using SOTS.Projectiles.Pyramid.Aten;

namespace SOTS.Items.Pyramid
{
    public class Aten : BaseFlailItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aten");
            Tooltip.SetDefault("Conjures stars that do 70% damage and explode for 210% damage\n'The defunct god... now in flail form'");
        }
        public override void SafeSetDefaults()
        {
            item.Size = new Vector2(34, 30);
            item.damage = 21;
            item.value = Item.sellPrice(0, 1, 50, 0);
            item.rare = ItemRarityID.LightRed;
            item.useTime = 30;
            item.useAnimation = 30;
            item.shoot = ModContent.ProjectileType<AtenProj>();
            item.shootSpeed = 14;
            item.knockBack = 4;
        }
    }
}