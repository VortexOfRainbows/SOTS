using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.GelGear
{
	[AutoloadEquip(EquipType.Head)]
	
	public class MusketeerHat : ModItem
	{	int Probe = -1;
		public override void SetDefaults()
		{

			item.width = 26;
			item.height = 16;

			item.value = 105000;
			item.rare = 5;
			item.defense = 1;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Musketeer's Helmet");
			Tooltip.SetDefault("25% increased range speed\n5% increased ranged damage\n33% decrease to all other damage types");
		}
		
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("MusketeerShirt") && legs.type == mod.ItemType("MusketeerLeggings");
        }
        public override void UpdateArmorSet(Player player)
        {	
			player.setBonus = "Converts all Musket Balls into Fusion Bolts";
			for(int i = 0; i < 1000; i++)
			{
				Projectile musketBall = Main.projectile[i];
				if(musketBall.type == 14)
				{
					musketBall.Kill();
					Vector2 projVelocity1 = new Vector2(musketBall.velocity.X, musketBall.velocity.Y).RotatedBy(MathHelper.ToRadians(45));
					Vector2 projVelocity2 = new Vector2(musketBall.velocity.X, musketBall.velocity.Y).RotatedBy(MathHelper.ToRadians(315));
					Projectile.NewProjectile(musketBall.Center.X, musketBall.Center.Y, projVelocity1.X * 0.35f, projVelocity1.Y * 0.35f, mod.ProjectileType("Fusion1"), (int)(musketBall.damage * 1f), musketBall.knockBack, Main.myPlayer);
					Projectile.NewProjectile(musketBall.Center.X, musketBall.Center.Y, projVelocity2.X * 0.35f, projVelocity2.Y * 0.35f, mod.ProjectileType("Fusion2"), (int)(musketBall.damage * 1f), musketBall.knockBack, Main.myPlayer);
					
				}
			}
			
		}
		

		public override void UpdateEquip(Player player)
		{
				player.rangedDamage += 0.05f;
				player.meleeDamage -= 0.33f;
				player.magicDamage -= 0.33f;
				player.minionDamage -= 0.33f;
				player.thrownDamage -= 0.33f;
                
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
                modPlayer.musketHat = true;
			
		}

	}
}