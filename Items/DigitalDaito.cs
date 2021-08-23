using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using Terraria.DataStructures;

namespace SOTS.Items
{
	public class DigitalDaito : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Digital Daito");
			Tooltip.SetDefault("WIP ITEM");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(2, 15));
		}
		public override void SetDefaults()
		{
            item.damage = 50;
            item.melee = true;  
            item.width = 62;
            item.height = 64;  
            item.useTime = 11; 
            item.useAnimation = 11;
            item.useStyle = 5;		
            item.knockBack = 8f;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Cyan;
            item.UseSound = null;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("DigitalSlash"); 
            item.shootSpeed = 18f;
			item.autoReuse = true;
            item.noUseGraphic = true; 
            item.noMelee = true;
		}
		int i = 0;
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			i++;
			Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI, i % 2 * 2 -1, Main.rand.NextFloat(0.875f, 1.125f));
			return false;
		}
		/*public override void GetVoid(Player player)
		{
			voidMana = 3;
		}*/
	}
}
