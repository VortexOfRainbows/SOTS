using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{
	public class SandstoneEdge : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sandstone Edge");
			Tooltip.SetDefault("Critical hits release a torrent of homing emerald bolts that do 50% damage");
		}
		public override void SetDefaults()
		{
			item.damage = 28;
			item.melee = true;
			item.width = 54;
			item.height = 54;
			item.useTime = 22;
			item.useAnimation = 22;
			item.useStyle = 1;
			item.knockBack = 3.5f;
			item.value = Item.sellPrice(0, 1, 20, 0);
			item.rare = 3;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}
		public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
		{
			if (crit && player.whoAmI == Main.myPlayer && !target.friendly && !target.immortal)
			{
				for (int i = 0; i < 6 + Main.rand.Next(6); i++)
				{
					Vector2 circularSpeed = new Vector2(2.5f, 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(120) + 30));
					Projectile.NewProjectile(player.Center.X, player.Center.Y, -circularSpeed.X, -circularSpeed.Y, mod.ProjectileType("EmeraldBoltHoming"), damage / 2 + 1, 3f, player.whoAmI);
				}
			}
		}
	}
}