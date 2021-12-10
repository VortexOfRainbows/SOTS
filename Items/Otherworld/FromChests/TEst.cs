using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Projectiles.Celestial;
using SOTS.Projectiles.Inferno;
using SOTS.NPCs.Boss.Curse;
using SOTS.Projectiles.Pyramid;
using SOTS.Buffs;
using SOTS.Projectiles.Slime;

namespace SOTS.Items.Otherworld.FromChests
{
	public class UndoArrow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("test");
			Tooltip.SetDefault("hi there");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.ThrowingKnife);
			item.damage = 17;
			item.thrown = true;
			item.rare = ItemRarityID.Green;
			item.autoReuse = true;            
			item.shoot = ModContent.ProjectileType<ShadeSpear>(); 
            item.shootSpeed = 1.0f;
			item.consumable = true;
		}
		int counter = 0;
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			SOTSPlayer.ModPlayer(player).UniqueVisionNumber = (SOTSPlayer.ModPlayer(player).UniqueVisionNumber + 1) % 24;
			return false; 
		}
	}
}