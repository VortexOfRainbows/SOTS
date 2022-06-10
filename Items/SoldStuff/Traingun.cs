using System;
using Microsoft.Xna.Framework;
using SOTS.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SoldStuff
{
	public class Traingun : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Train Gun");
			Tooltip.SetDefault("");
		}
        public override void SetDefaults()
        {
            Item.damage = 75;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 90;
            Item.height = 62;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.value = Item.buyPrice(5, 0, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.UseSound = SoundID.Item61;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Trains>();
            Item.shootSpeed = 14f;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, -2);
        }
	}
}
