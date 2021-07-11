using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Projectiles.Celestial;
using SOTS.Projectiles.Inferno;
using SOTS.NPCs.Boss.Curse;
using SOTS.Projectiles.Pyramid;

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
			for(int i = 0; i < Main.maxNPCs; i++)
            {
				NPC npc = Main.npc[i];
				if(npc.active && npc.type == ModContent.NPCType<PharaohsCurse>())
				{
					Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, npc.whoAmI, 0);
					break;
				}
            }
			return false; 
		}
	}
}