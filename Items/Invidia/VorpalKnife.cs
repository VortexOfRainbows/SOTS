using Terraria;
using SOTS.Void;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using SOTS.Projectiles.Blades;
using System;
using SOTS.Projectiles.Lightning;

namespace SOTS.Items.Invidia
{
	public class VorpalKnife : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vorpal Knife");
			Tooltip.SetDefault("Toss a blade that lingers in the air\nDetonate with left click, dealing 300% damage\nTeleport with right click, slashing for 200% damage\nDrains more void to detonate or teleport the farther away the blade is");//\n'It originally served a purpose as a weapon of sacrificial beheading'");
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
			Item.DamageType = DamageClass.Melee;
			Item.damage = 21;
			Item.width = 50;
			Item.height = 48;
            Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.useTime = 33;
			Item.useAnimation = 33;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.UseSound = SoundID.Item1;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.autoReuse = true;
			Item.knockBack = 1.5f;
			Item.shoot = ModContent.ProjectileType<VorpalThrow>();
			Item.shootSpeed = 12;
		}
        public override bool BeforeDrainMana(Player player)
        {
			Item.useTime = Item.useAnimation;
            return true;
        }
        public override bool AltFunctionUse(Player player)
        {
			return distanceFromBlade(player) > 32 && !player.mount.Active;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.ItemUsesThisAnimation > 1)
				return false;
			for(int i = 0; i < 1000; i++)
            {
				Projectile proj = Main.projectile[i];
				if(proj.type == type && proj.active && proj.owner == player.whoAmI)
				{
					if (player.altFunctionUse == 2)
                    {
						proj.Kill();
                    }
					else
					{
						proj.timeLeft = 90;
					}
					return false;
				}
            }
			if (player.altFunctionUse == 2)
				return false;
			Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI, 0, 0.6f);
			return false;
        }
		public float distanceFromBlade(Player player)
		{
			for (int i = 0; i < 1000; i++)
			{
				Projectile proj = Main.projectile[i];
				if (proj.type == Item.shoot && proj.active && proj.owner == player.whoAmI && proj.timeLeft < 180)
				{
					VorpalThrow vThrow = proj.ModProjectile as VorpalThrow;
					return (int)(vThrow.newCenter - player.Center).Length();
				}
			}
			return -1;
		}
        public override int GetVoid(Player player)
		{
			int additional = 0;
			float fromBlade = distanceFromBlade(player);
			if(fromBlade > -1)
            {
				float defaultGain = fromBlade / 160f; //1 additional void for every 10 blocks
				if(defaultGain > 6) //5 additional void at 50 blocks
                {
					defaultGain = 6;
					float exponentialGain = ((fromBlade - 960) / 160f); //1 additional void for every 10 blocks
					exponentialGain *= exponentialGain; //then squared... This means that it will be 6 additional void at 60 blocks, but then will grow exponentially.
					if(exponentialGain > 25) //
                    {
						exponentialGain = 25;
						float logarithmicGain = (float)Math.Log2(1 + (fromBlade - 1760f) / 160f);
						exponentialGain += logarithmicGain;
                    }
					defaultGain += exponentialGain;
                }
				additional = (int)defaultGain;
            }
			return 4 + additional;
        }
    }
}