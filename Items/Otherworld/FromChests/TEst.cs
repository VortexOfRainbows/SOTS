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
			item.CloneDefaults(279);
			item.damage = 17;
			item.thrown = true;
			item.rare = 2;
			item.autoReuse = true;            
			item.shoot = ModContent.ProjectileType<ShadeSpear>(); 
            item.shootSpeed = 1.0f;
			item.consumable = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			position = Main.MouseWorld;
			Projectile.NewProjectile(position, new Vector2(0, -1.5f), ModContent.ProjectileType<SubspaceDeathAnimation>(), damage, knockBack, player.whoAmI, 0, 0);
			return false; 
		}
	}
}