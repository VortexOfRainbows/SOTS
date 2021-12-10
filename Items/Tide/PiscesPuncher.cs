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
            item.damage = 21;
            item.magic = true;
            item.width = 64;
            item.height = 30;
            item.useTime = 29; 
            item.useAnimation = 29;
            item.useStyle = ItemUseStyleID.HoldingOut;    
            item.noMelee = true;
			item.knockBack = 6f;  
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = ItemRarityID.Orange;
            item.UseSound = SoundID.Item61;
            item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<FishBullet>(); 
            item.shootSpeed = 12.5f;
			item.mana = 12;
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
