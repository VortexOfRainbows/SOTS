using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Projectiles.Pyramid.Aten;

namespace SOTS.Items.Flails
{
    public class Aten : BaseFlailItem
    {
        public override void SetStaticDefaults()
        {
            this.SetResearchCost(1);
        }
        public override void SafeSetDefaults()
        {
            Item.Size = new Vector2(44, 46);
            Item.damage = 21;
            Item.value = Item.sellPrice(0, 1, 50, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.shoot = ModContent.ProjectileType<AtenProj>();
            Item.shootSpeed = 14;
            Item.knockBack = 4;
            Item.channel = true;
        }
    }
}