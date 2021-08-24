using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Void;

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
            item.damage = 17;
            item.ranged = true;
            item.width = 38;
            item.height = 36;
            item.useTime = 50; 
            item.useAnimation = 50;
            item.useStyle = 5;    
            item.noMelee = true;
			item.knockBack = 6f;  
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = ItemRarityID.Blue;
            item.UseSound = SoundID.Item61;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("Coconut"); 
            item.shootSpeed = 9.5f;
		}
		public override void GetVoid(Player player)
		{
			voidMana = 24;
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
