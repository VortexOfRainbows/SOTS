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
            item.damage = 75;
            item.ranged = true;
            item.width = 90;
            item.height = 62;
            item.useTime = 24;
            item.useAnimation = 24;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 5f;
            item.value = Item.buyPrice(5, 0, 0, 0);
            item.rare = ItemRarityID.Cyan;
            item.UseSound = SoundID.Item61;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<Trains>();
            item.shootSpeed = 14f;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, -2);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return true; 
	    }
	}
}
