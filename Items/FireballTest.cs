using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Projectiles;

namespace SOTS.Items
{
    public class FireballTest : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fireball Test");
            Tooltip.SetDefault("Test weapon to summon a fireball");
        }
        public override void SetDefaults()
        {
            item.damage = 45;
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
            item.shoot = ModContent.ProjectileType<TestFireball>();
            item.shootSpeed = 5;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, -4);
        }
    }
}
