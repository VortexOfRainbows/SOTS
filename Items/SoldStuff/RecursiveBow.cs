using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SoldStuff
{
	public class RecursiveBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
        }
		public override void SetDefaults()
		{
            Item.damage = 14;  
            Item.DamageType = DamageClass.Ranged; 
            Item.width = 22; 
            Item.height = 64;
            Item.useTime = 33;
            Item.useAnimation = 33;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.noMelee = true;
            Item.knockBack = 4.5f;
            Item.value = Item.buyPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = 10;
            Item.shootSpeed = 14.5f;
            Item.useAmmo = AmmoID.Arrow;
		}
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-0.5f, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int totalNumber = 0;
            while(Main.rand.NextBool(2))
            {
                totalNumber++;
                //float speed = velocity.Length();
                float originalSpeedMult = 1f - 0.05f * totalNumber;
                if (originalSpeedMult < 0.5f)
                    originalSpeedMult = 0.5f;
                float topRadius = (float)Math.Sqrt(2f * totalNumber);
                Vector2 randomSpread = Main.rand.NextVector2Circular(topRadius, topRadius);
                Projectile.NewProjectile(source, position, velocity * originalSpeedMult + randomSpread, type, damage, knockback, player.whoAmI);
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
