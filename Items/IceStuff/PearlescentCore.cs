using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;

namespace SOTS.Items.IceStuff
{
	public class PearlescentCore : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pearlescent Core");
			Tooltip.SetDefault("Every 8th magic attack launches an additional laser projectile that does massive damage\nSome weapons won't trigger the effect\nIncreases max summons by 1\nWorks while in the inventory\nDecreases void regen by 3.25\n'The core of the artificial horror'");
		}
		public override void SetDefaults()
		{
			item.magic = true;
			item.width = 30;
			item.height = 38;
			item.maxStack = 1;
			item.consumable = false; 
			item.knockBack = 0.1f;
            item.value = Item.sellPrice(0, 1, 75, 0);
			item.rare = 7;
			item.expert = true;
		}
		float rotation = 0;
		int Probe = -1;
		public override void UpdateInventory(Player player)
		{
				VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
				SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
				if(!modPlayer.pearlescentMagic)
				{
					rotation += 0.75f;
					int type = mod.ProjectileType("PearlescentCore");
					if (Probe == -1)
					{
						Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, type, item.damage, 0, player.whoAmI); //waterbolt proj
						Projectile proj = Main.projectile[Probe];
						proj.ai[1] = 0;
					}
					if (!Main.projectile[Probe].active || Main.projectile[Probe].type != type)
					{
						Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, type, item.damage, 0, player.whoAmI);
						Projectile proj = Main.projectile[Probe];
						proj.ai[1] = 0;
					}
					Main.projectile[Probe].timeLeft = 6;
					if (Probe != -1)
					{
						Projectile proj = Main.projectile[Probe];
						proj.penetrate = -1;
						Vector2 initialLoop = new Vector2(72, 0).RotatedBy(MathHelper.ToRadians(rotation));
						initialLoop.X /= 2.25f;
						proj.position.X = initialLoop.X + player.Center.X - proj.width/2;
						proj.position.Y = initialLoop.Y + player.Center.Y - proj.height/2;
					}
					voidPlayer.voidRegen -= 0.325f;
					player.maxMinions++;
					modPlayer.pearlescentMagic = true;
				}
		}
	}
}