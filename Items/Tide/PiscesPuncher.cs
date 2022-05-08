using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Void;
using SOTS.Projectiles.Tide;

namespace SOTS.Items.Tide
{
	public class PiscesPuncher : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pisces Puncher");
			Tooltip.SetDefault("'fish'\n'noun'\n'a limbless cold-blooded vertebrate animal with gills and fins and living wholly in water'");
		}
		public override void SetDefaults()
		{
            Item.damage = 21;
            Item.magic = true;
            Item.width = 64;
            Item.height = 30;
            Item.useTime = 29; 
            Item.useAnimation = 29;
            Item.useStyle = ItemUseStyleID.HoldingOut;    
            Item.noMelee = true;
			Item.knockBack = 6f;  
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item61;
            Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<FishBullet>(); 
            Item.shootSpeed = 12.5f;
			Item.mana = 12;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-12, 0);
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			for(int i = -1; i <= 1; i++)
			{
				Vector2 velocity = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(10 * i)) + Main.rand.NextVector2Circular(2, 2);
				Projectile.NewProjectile(position + velocity.SafeNormalize(Vector2.Zero) * 28, velocity, type, damage, knockBack, player.whoAmI, ChooseItem(), i);
			}
			return false;
		}
		public int ChooseItem()
		{
			int item = SOTSItem.piscesFishItems[Main.rand.Next(SOTSItem.piscesFishItems.Length)];
			if (System.DateTime.Now.Day == 1 && System.DateTime.Now.Month == 4)
				item = ItemID.Bunny;
			return item;
		}
	}
}
