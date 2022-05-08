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
			Item.damage = 23;
			Item.DamageType = DamageClass.Melee;
			Item.width = 46;
			Item.height = 50;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = 5;
			Item.knockBack = 5;
			Item.value = Item.sellPrice(0, 1, 20, 0);
			Item.rare = 3;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = mod.ProjectileType("PyramidSpear");
			Item.shootSpeed = 5.5f;
			Item.noUseGraphic = true;
			Item.noMelee = true;
		}
		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}
	}
}