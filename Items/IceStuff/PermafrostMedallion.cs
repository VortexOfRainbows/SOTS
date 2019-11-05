using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;


namespace SOTS.Items.IceStuff
{
	public class PermafrostMedallion : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Permafrost Medallion");
			Tooltip.SetDefault("Surrounds you with a blizzard of artifact probes");
		}
		public override void SetDefaults()
		{
	
			item.damage = 32;
			item.summon = true;
            item.width = 28;     
            item.height = 36;   
            item.value = Item.sellPrice(0, 5, 50, 0);
            item.rare = 7;
			item.accessory = true;

		}
		float rotation = 0;
		float rotation2 = 0;
		
		int Probe = -1;
		int Probe2 = -1;
		int Probe3 = -1;
		int Probe4 = -1;
		int Probe5 = -1;
		int Probe6 = -1;
		int Probe7 = -1;
		int Probe8 = -1;
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			rotation += 2.15f;
			rotation2 += 2.15f;
			int type = mod.ProjectileType("BlizzardProbe");
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
					proj.tileCollide = false;
					proj.penetrate = -1;
					Vector2 initialLoop = new Vector2(164, 0).RotatedBy(MathHelper.ToRadians(rotation));
					initialLoop.X /= 2.0f;
					Vector2 properLoop = new Vector2(initialLoop.X, initialLoop.Y).RotatedBy(MathHelper.ToRadians(rotation2));
					proj.position.X = properLoop.X + player.Center.X - proj.width/2;
					proj.position.Y = properLoop.Y + player.Center.Y - proj.height/2;
				}
				
				if (Probe2 == -1)
				{
					Probe2 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, type, item.damage, 0, player.whoAmI);
					Projectile proj = Main.projectile[Probe2];
					proj.ai[1] = 15;
				}
				if (!Main.projectile[Probe2].active || Main.projectile[Probe2].type != type)
				{
					Probe2 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, type, item.damage, 0, player.whoAmI);
					Projectile proj = Main.projectile[Probe2];
					proj.ai[1] = 15;
				}
				Main.projectile[Probe2].timeLeft = 6;
				if (Probe2 != -1)
				{
					Projectile proj = Main.projectile[Probe2];
					proj.tileCollide = false;
					proj.penetrate = -1;
					Vector2 initialLoop = new Vector2(164, 0).RotatedBy(MathHelper.ToRadians(rotation + 45));
					initialLoop.Y /= 2.0f;
					Vector2 properLoop = new Vector2(initialLoop.X, initialLoop.Y).RotatedBy(MathHelper.ToRadians(rotation2));
					proj.position.X = properLoop.X + player.Center.X - proj.width/2;
					proj.position.Y = properLoop.Y + player.Center.Y - proj.height/2;
				}
				if (Probe3 == -1)
				{
					Probe3 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, type, item.damage, 0, player.whoAmI); //waterbolt proj
					Projectile proj = Main.projectile[Probe3];
					proj.ai[1] = 30;
				}
				if (!Main.projectile[Probe3].active || Main.projectile[Probe3].type != type)
				{
					Probe3 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, type, item.damage, 0, player.whoAmI);
					Projectile proj = Main.projectile[Probe3];
					proj.ai[1] = 30;
				}
				Main.projectile[Probe3].timeLeft = 6;
				if (Probe3 != -1)
				{
					Projectile proj = Main.projectile[Probe3];
					proj.tileCollide = false;
					proj.penetrate = -1;
					Vector2 initialLoop = new Vector2(164, 0).RotatedBy(MathHelper.ToRadians(rotation + 90));
					initialLoop.X /= 2.0f;
					Vector2 properLoop = new Vector2(initialLoop.X, initialLoop.Y).RotatedBy(MathHelper.ToRadians(rotation2));
					proj.position.X = properLoop.X + player.Center.X - proj.width/2;
					proj.position.Y = properLoop.Y + player.Center.Y - proj.height/2;
				}
				
				if (Probe4 == -1)
				{
					Probe4 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, type, item.damage, 0, player.whoAmI);
					Projectile proj = Main.projectile[Probe4];
					proj.ai[1] = 45;
				}
				if (!Main.projectile[Probe4].active || Main.projectile[Probe4].type != type)
				{
					Probe4 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, type, item.damage, 0, player.whoAmI);
					Projectile proj = Main.projectile[Probe4];
					proj.ai[1] = 45;
				}
				Main.projectile[Probe4].timeLeft = 6;
				if (Probe4 != -1)
				{
					Projectile proj = Main.projectile[Probe4];
					proj.tileCollide = false;
					proj.penetrate = -1;
					Vector2 initialLoop = new Vector2(164, 0).RotatedBy(MathHelper.ToRadians(rotation + 135));
					initialLoop.Y /= 2.0f;
					Vector2 properLoop = new Vector2(initialLoop.X, initialLoop.Y).RotatedBy(MathHelper.ToRadians(rotation2));
					proj.position.X = properLoop.X + player.Center.X - proj.width/2;
					proj.position.Y = properLoop.Y + player.Center.Y - proj.height/2;
				}
				if (Probe5 == -1)
				{
					Probe5 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, type, item.damage, 0, player.whoAmI); //waterbolt proj
					Projectile proj = Main.projectile[Probe5];
					proj.ai[1] = 60;
				}
				if (!Main.projectile[Probe5].active || Main.projectile[Probe5].type != type)
				{
					Probe5 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, type, item.damage, 0, player.whoAmI);
					Projectile proj = Main.projectile[Probe5];
					proj.ai[1] = 60;
				}
				Main.projectile[Probe5].timeLeft = 6;
				if (Probe5 != -1)
				{
					Projectile proj = Main.projectile[Probe5];
					proj.tileCollide = false;
					proj.penetrate = -1;
					Vector2 initialLoop = new Vector2(164, 0).RotatedBy(MathHelper.ToRadians(rotation + 180));
					initialLoop.X /= 2.0f;
					Vector2 properLoop = new Vector2(initialLoop.X, initialLoop.Y).RotatedBy(MathHelper.ToRadians(rotation2));
					proj.position.X = properLoop.X + player.Center.X - proj.width/2;
					proj.position.Y = properLoop.Y + player.Center.Y - proj.height/2;
				}
				
				if (Probe6 == -1)
				{
					Probe6 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, type, item.damage, 0, player.whoAmI);
					Projectile proj = Main.projectile[Probe6];
					proj.ai[1] = 75;
				}
				if (!Main.projectile[Probe6].active || Main.projectile[Probe6].type != type)
				{
					Probe6 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, type, item.damage, 0, player.whoAmI);
					Projectile proj = Main.projectile[Probe6];
					proj.ai[1] = 75;
				}
				Main.projectile[Probe6].timeLeft = 6;
				if (Probe6 != -1)
				{
					Projectile proj = Main.projectile[Probe6];
					proj.tileCollide = false;
					proj.penetrate = -1;
					Vector2 initialLoop = new Vector2(164, 0).RotatedBy(MathHelper.ToRadians(rotation + 225));
					initialLoop.Y /= 2.0f;
					Vector2 properLoop = new Vector2(initialLoop.X, initialLoop.Y).RotatedBy(MathHelper.ToRadians(rotation2));
					proj.position.X = properLoop.X + player.Center.X - proj.width/2;
					proj.position.Y = properLoop.Y + player.Center.Y - proj.height/2;
				}
				if (Probe7 == -1)
				{
					Probe7 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, type, item.damage, 0, player.whoAmI); //waterbolt proj
					Projectile proj = Main.projectile[Probe7];
					proj.ai[1] = 90;
				}
				if (!Main.projectile[Probe7].active || Main.projectile[Probe7].type != type)
				{
					Probe7 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, type, item.damage, 0, player.whoAmI);
					Projectile proj = Main.projectile[Probe7];
					proj.ai[1] = 90;
				}
				Main.projectile[Probe7].timeLeft = 6;
				if (Probe7 != -1)
				{
					Projectile proj = Main.projectile[Probe7];
					proj.tileCollide = false;
					proj.penetrate = -1;
					Vector2 initialLoop = new Vector2(164, 0).RotatedBy(MathHelper.ToRadians(rotation + 270));
					initialLoop.X /= 2.0f;
					Vector2 properLoop = new Vector2(initialLoop.X, initialLoop.Y).RotatedBy(MathHelper.ToRadians(rotation2));
					proj.position.X = properLoop.X + player.Center.X - proj.width/2;
					proj.position.Y = properLoop.Y + player.Center.Y - proj.height/2;
				}
				
				if (Probe8 == -1)
				{
					Probe8 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, type, item.damage, 0, player.whoAmI);
					Projectile proj = Main.projectile[Probe8];
					proj.ai[1] = 105;
				}
				if (!Main.projectile[Probe8].active || Main.projectile[Probe8].type != type)
				{
					Probe8 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, type, item.damage, 0, player.whoAmI);
					Projectile proj = Main.projectile[Probe8];
					proj.ai[1] = 105;
				}
				Main.projectile[Probe8].timeLeft = 6;
				if (Probe8 != -1)
				{
					Projectile proj = Main.projectile[Probe8];
					proj.tileCollide = false;
					proj.penetrate = -1;
					Vector2 initialLoop = new Vector2(164, 0).RotatedBy(MathHelper.ToRadians(rotation + 315));
					initialLoop.Y /= 2.0f;
					Vector2 properLoop = new Vector2(initialLoop.X, initialLoop.Y).RotatedBy(MathHelper.ToRadians(rotation2));
					proj.position.X = properLoop.X + player.Center.X - proj.width/2;
					proj.position.Y = properLoop.Y + player.Center.Y - proj.height/2;
				}
		}
	}
}