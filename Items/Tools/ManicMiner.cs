using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using SOTS.Void;
using SOTS.Projectiles;
using Terraria.ModLoader;

namespace SOTS.Items.Tools
{
	public class ManicMiner : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Manic Miner");
			Tooltip.SetDefault("Converts void into mining lasers");
		}
		public override void SafeSetDefaults()
		{
			item.width = 34;
			item.height = 26;
			item.useTime = 24;
			item.useAnimation = 24;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 5;
            item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item12;
			item.autoReuse = true;            
			item.shoot = ModContent.ProjectileType<ManaMiner>(); 
            item.shootSpeed = 5.75f;
		}
		public override int GetVoid(Player player)
		{
			return  6;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			return true; 
		}
	}
}