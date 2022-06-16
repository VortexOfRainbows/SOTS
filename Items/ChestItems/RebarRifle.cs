using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Projectiles.BiomeChest;

namespace SOTS.Items.ChestItems
{
    public class RebarRifle : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rebar Rifle");
            Tooltip.SetDefault("Shoots a powerful, high velocity rebar that applies a stacking, permanent bleed to hit enemies for 5 damage per second");
            this.SetResearchCost(1);
        }
        public override void SetDefaults()
        {
            Item.damage = 135;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 68;
            Item.height = 24;
            Item.useTime = 21;
            Item.useAnimation = 21;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3;
            Item.value = Item.sellPrice(0, 20, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item99;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Rebar>();
            Item.shootSpeed = 4.75f;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-1, 0);
        }
    }
}
