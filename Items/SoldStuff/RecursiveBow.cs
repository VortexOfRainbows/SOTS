using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SoldStuff
{
	public class RecursiveBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Recursive Bow");
			Tooltip.SetDefault("Has a 50% chance to fire an additional arrow\nAdditional arrows also activate this ability");
		}
		public override void SetDefaults()
		{
            item.damage = 14;  
            item.ranged = true; 
            item.width = 22; 
            item.height = 64;
            item.useTime = 33;
            item.useAnimation = 33;
            item.useStyle = ItemUseStyleID.HoldingOut;    
            item.noMelee = true;
            item.knockBack = 4.5f;
            item.value = Item.buyPrice(0, 15, 0, 0);
            item.rare = ItemRarityID.Orange;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shoot = 10;
            item.shootSpeed = 14.5f;
            item.useAmmo = AmmoID.Arrow;
		}
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-0.5f, 0);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int totalNumber = 0;
            while(Main.rand.NextBool(2))
            {
                totalNumber++;
                float speed = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
                float originalSpeedMult = 1f - 0.05f * totalNumber;
                if (originalSpeedMult < 0.5f)
                    originalSpeedMult = 0.5f;
                float topRadius = (float)Math.Sqrt(2f * totalNumber);
                Vector2 randomSpread = Main.rand.NextVector2Circular(topRadius, topRadius);
                Projectile.NewProjectile(position, new Vector2(speedX, speedY) * originalSpeedMult + randomSpread, type, damage, knockBack, player.whoAmI);
            }
            if(totalNumber >= 1)
            {
                double chance = 1 / (Math.Pow(2, totalNumber));
                chance *= 100;
                chance = (float)chance;
                int g = 167 - totalNumber * 12;
                int b = 66 - totalNumber * 6;
                if (g < 0) g = 0;
                if (b < 0) b = 0;
                CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), new Color(255, g, b, 255), string.Concat(chance + "%"), false, false);
            }
            return true; 
		}
	}
}
