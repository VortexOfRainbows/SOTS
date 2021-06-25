using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using Terraria.DataStructures;

namespace SOTS.Items
{
	public class DigitalDaito : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Digital Daito");
			Tooltip.SetDefault("WIP ITEM");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(2, 15));
		}
		public override void SafeSetDefaults()
		{
            item.damage = 50;
            item.melee = true;  
            item.width = 62;
            item.height = 64;  
            item.useTime = 10; 
            item.useAnimation = 10;
            item.useStyle = 5;		
            item.knockBack = 8f;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Cyan;
            item.UseSound = SoundID.Item22;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("DigitalSlash"); 
            item.shootSpeed = 18f;
			item.autoReuse = true;
            item.noUseGraphic = true; 
            item.noMelee = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			return true;
		}
		public override void GetVoid(Player player)
		{
			voidMana = 5;
		}
	}
}
