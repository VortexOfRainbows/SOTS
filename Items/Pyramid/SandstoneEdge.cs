using Microsoft.Xna.Framework;
using SOTS.Projectiles.Pyramid;
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
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.crit = 10;
			Item.damage = 30;
			Item.scale = 1.2f;
			Item.DamageType = DamageClass.Melee;
			Item.useTurn = true;
			Item.width = 46;
			Item.height = 52;
			Item.useTime = 22;
			Item.useAnimation = 22;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 3.5f;
			Item.value = Item.sellPrice(0, 1, 20, 0);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.scale = 1.4f;
		}
		public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
		{
			if (crit && player.whoAmI == Main.myPlayer && !target.friendly && !target.immortal)
			{
				for (int i = 0; i < 3 + Main.rand.Next(3); i++)
				{
					Vector2 circularSpeed = new Vector2(2.5f, 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(120) + 30));
					Projectile.NewProjectile(player.GetSource_OnHit(target), player.Center.X, player.Center.Y, -circularSpeed.X, -circularSpeed.Y, ModContent.ProjectileType<EmeraldBoltHoming>(), (int)(damage * 0.5f) + 1, 3f, player.whoAmI);
				}
			}
		}
	}
}