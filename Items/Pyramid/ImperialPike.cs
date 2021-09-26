using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{
	public class ImperialPike : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Imperial Pike");
			Tooltip.SetDefault("Leaves behind a trail that continues to damage enemies");
		}
		public override void SetDefaults()
		{
			item.damage = 23;
			item.melee = true;
			item.width = 46;
			item.height = 50;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 5;
			item.knockBack = 5;
			item.value = Item.sellPrice(0, 1, 20, 0);
			item.rare = 3;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("PyramidSpear");
			item.shootSpeed = 5.5f;
			item.noUseGraphic = true;
			item.noMelee = true;
		}
		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[item.shoot] < 1;
		}
	}
}