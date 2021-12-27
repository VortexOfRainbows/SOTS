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
			item.useTime = 3;
			item.thrown = true;
			item.rare = ItemRarityID.Green;
			item.autoReuse = true;            
			item.shoot = ModContent.ProjectileType<LavaLaser>(); 
            item.shootSpeed = 3.0f;
			item.consumable = true;
		}
		int counter = 0;
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, Main.myPlayer, Main.rand.NextFloat(0.65f, 0.75f), Main.rand.NextFloat(360));
			//SOTSPlayer.ModPlayer(player).UniqueVisionNumber = (SOTSPlayer.ModPlayer(player).UniqueVisionNumber + 1) % 24;
			return false; 
		}
	}
}