using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Projectiles;

namespace SOTS.Items.ChestItems
{
	public class SpikedClub : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spiked Club");
			Tooltip.SetDefault("Lays down spike traps\nLays down more traps when wearing climbing related accessories");
		}
		public override void SetDefaults()
		{
			item.damage = 12;
			item.melee = true;
			item.width = 52;
			item.height = 52;
			item.useTime = 38;
			item.useAnimation = 38;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 3;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;            
			item.shoot = ModContent.ProjectileType<SpikeTrap>(); 
            item.shootSpeed = 3.5f;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			for(int i = 0; i < player.spikedBoots; i++)
			{
				float speedMult = 1.25f + i * 0.25f;
				Projectile.NewProjectile(position.X, position.Y, speedX * speedMult, speedY * speedMult, type, damage, knockBack, player.whoAmI);
			}
			return true; 
		}
	}
}