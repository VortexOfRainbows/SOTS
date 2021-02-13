using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Projectiles.BiomeChest;

namespace SOTS.Items
{
    public class RebarRifle : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rebar Rifle");
            Tooltip.SetDefault("Shoots a powerful, high velocity rebar that applies a stacking, permanent bleed to hit enemies for 5 damage per second");
        }
        public override void SetDefaults()
        {
            item.damage = 135;
            item.ranged = true;
            item.width = 68;
            item.height = 24;
            item.useTime = 21;
            item.useAnimation = 21;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 3;
            item.value = Item.sellPrice(0, 20, 0, 0);
            item.rare = ItemRarityID.Yellow;
            item.UseSound = SoundID.Item99;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<Rebar>();
            item.shootSpeed = 4.75f;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-1, 0);
        }
    }
}
