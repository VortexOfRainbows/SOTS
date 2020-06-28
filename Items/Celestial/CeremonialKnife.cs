using Terraria;
using SOTS.Void;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Celestial
{
	public class CeremonialKnife : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ceremonial Knife");
			Tooltip.SetDefault("Critical hits steal life");
		}
		public override void SafeSetDefaults()
		{
			item.melee = true;
			item.damage = 66;
			item.width = 26;
			item.height = 32;
            item.value = Item.sellPrice(0, 10, 0, 0);
			item.rare = 8;
			item.useTime = 7;
			item.useAnimation = 14;
			item.useStyle = 1;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.shoot = mod.ProjectileType("CeremonialSlash"); 
			item.shootSpeed = 14;
			item.knockBack = 2f;
			item.expert = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(15));
			Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI, Main.rand.Next(28));
			return false;
		}
		public override void GetVoid(Player player)
		{
			voidMana = 4;
		}
	}
}