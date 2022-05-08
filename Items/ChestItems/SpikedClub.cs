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
			Item.damage = 12;
			Item.melee = true;
			Item.width = 52;
			Item.height = 52;
			Item.useTime = 38;
			Item.useAnimation = 38;
			Item.useStyle = ItemUseStyleID.SwingThrow;
			Item.knockBack = 3;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;            
			Item.shoot = ModContent.ProjectileType<SpikeTrap>(); 
            Item.shootSpeed = 3.5f;
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