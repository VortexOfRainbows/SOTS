using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Void;
using SOTS.Projectiles.Tide;

namespace SOTS.Items.ChestItems
{
	public class CoconutGun : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coconut Gun");
			Tooltip.SetDefault("Launches coconut cluster bombs with homing coconut milk shrapnel\n'It fires in spurts'");
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 17;
            Item.ranged = true;
            Item.width = 38;
            Item.height = 36;
            Item.useTime = 50; 
            Item.useAnimation = 50;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.noMelee = true;
			Item.knockBack = 6f;  
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item61;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Coconut>(); 
            Item.shootSpeed = 9.5f;
		}
		public override int GetVoid(Player player)
		{
			return 24;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-1, -2);
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI, 3);
			return false; 
		}
	}
}
