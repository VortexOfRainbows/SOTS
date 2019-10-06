using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.ChestItems
{
	public class CryoCannon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryo Cannon");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
            item.damage = 7;
            item.ranged = true;
            item.width = 34;
            item.height = 30;
            item.useTime = 30; 
            item.useAnimation = 30;
            item.useStyle = 5;    
            item.noMelee = true;
			item.knockBack = 1.6f;  
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = 2;
            item.UseSound = SoundID.Item61;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("IceCluster"); 
            item.shootSpeed = 13;
			item.useAmmo = ItemID.Snowball;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(7, 0);
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
              int numberProjectiles = 1;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(3));
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("IceCluster"), damage, knockBack, player.whoAmI);
              }
              return false; 
		}
	}
}
