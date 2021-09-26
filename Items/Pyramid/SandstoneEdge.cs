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
			item.crit = 10;
			item.damage = 30;
			item.scale = 1.2f;
			item.melee = true;
			item.useTurn = true;
			item.width = 54;
			item.height = 54;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 3.5f;
			item.value = Item.sellPrice(0, 1, 20, 0);
			item.rare = ItemRarityID.Orange;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}
		public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
		{
			if (crit && player.whoAmI == Main.myPlayer && !target.friendly && !target.immortal)
			{
				for (int i = 0; i < 3 + Main.rand.Next(3); i++)
				{
					Vector2 circularSpeed = new Vector2(2.5f, 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(120) + 30));
					Projectile.NewProjectile(player.Center.X, player.Center.Y, -circularSpeed.X, -circularSpeed.Y, mod.ProjectileType("EmeraldBoltHoming"), (int)(damage * 0.5f) + 1, 3f, player.whoAmI);
				}
			}
		}
	}
}