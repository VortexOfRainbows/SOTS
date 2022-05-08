using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Pyramid
{
	public class ShiftingSands : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shifting Sands");
			Tooltip.SetDefault("Pushes back nearby enemies with a wave of sand");
		}
		public override void SetDefaults()
		{
            Item.damage = 16;
            Item.magic = true; 
            Item.width = 34;    
            Item.height = 34; 
            Item.useTime = 23; 
            Item.useAnimation = 23;
            Item.useStyle = 5;    
            Item.knockBack = 6.5f;
            Item.value = Item.sellPrice(0, 1, 20, 0);
            Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item34;
            Item.noMelee = true; 
            Item.autoReuse = true;
            Item.shootSpeed = 2f; 
			Item.shoot = mod.ProjectileType("SandPuff");
			Item.mana = 16;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            float numberProjectiles = 12; 
            float rotation = MathHelper.ToRadians(30);
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(rotation * i);
                Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
            }
            return false;
		}
	}
}
