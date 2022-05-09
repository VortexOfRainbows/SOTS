using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Slime
{
    public class FireSpitter : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fire Spitter");
            Tooltip.SetDefault("Uses gel for ammo");
        }
        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 44;
            Item.height = 24;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 0;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.Flames;
            Item.shootSpeed = 5.5f;
            Item.useAmmo = 23; 
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-0.5f, 0);
        }
    }
}
