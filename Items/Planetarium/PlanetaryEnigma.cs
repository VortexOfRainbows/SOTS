using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace SOTS.Items.Planetarium
{
	public class PlanetaryEnigma : ModItem
	{	int timer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Planetary Enigma");
			Tooltip.SetDefault("Chooses a random enemy to erupt every couple of seconds\nErupts enemies into 8 planetary fires\nDamage scales with enemy health and defense\nDamage caps at 120 per fire");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(7, 4));
		}
		public override void SetDefaults()
		{
      
            item.width = 46;     
            item.height = 46;   
            item.value = 150000;
            item.rare = 9;
			item.accessory = true;

		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			float active = 0;
			timer++;
			if(timer >= 90)
			{
				for(int i = Main.rand.Next(200); active <= 1f; i = Main.rand.Next(200))
				{
					active += 0.025f;
					timer = 0;
					NPC npc = Main.npc[i];
					if((npc.active == true && npc.friendly == false))
					{
						active = 2;
						float Xcen = npc.Center.X;
						float Ycen = npc.Center.Y;
						float Xpos = npc.position.X;
						float Ypos = npc.position.Y;
						int Damage = (int)(npc.life/24);
						Damage += (int)(npc.lifeMax/60);
						Damage += (int)(npc.defense/10);
						Damage += 5;
						if(npc.boss == true)
						{
							Damage -= 10;
						}
						if(Damage > 120)
						{
							Damage = 120;
						}
						Projectile.NewProjectile(Xcen, Ypos - 20, 0, -5, mod.ProjectileType("PlanetaryFlame2"), Damage, 0, player.whoAmI);
						Projectile.NewProjectile(Xpos - 20, Ycen, -5, 0, mod.ProjectileType("PlanetaryFlame2"), Damage, 0, player.whoAmI);
						Projectile.NewProjectile(Xpos + -(2 * (Xpos - Xcen)) + 20, Ycen, +5, 0, mod.ProjectileType("PlanetaryFlame2"), Damage, 0, player.whoAmI);
						Projectile.NewProjectile(Xcen, Ypos + -(2 * (Ypos - Ycen)) + 20, 0, +5, mod.ProjectileType("PlanetaryFlame2"), Damage, 0, player.whoAmI);
						Projectile.NewProjectile(Xpos + -(2 * (Xpos - Xcen)) + 20, Ypos - 20, +5, -5, mod.ProjectileType("PlanetaryFlame2"), Damage, 0, player.whoAmI);
						Projectile.NewProjectile(Xpos - 20, Ypos - 20, -5, -5, mod.ProjectileType("PlanetaryFlame2"), Damage, 0, player.whoAmI);
						Projectile.NewProjectile(Xpos + -(2 * (Xpos - Xcen)) + 20, Ypos + -(2 * (Ypos - Ycen)) + 20, +5, +5, mod.ProjectileType("PlanetaryFlame2"), Damage, 0, player.whoAmI);
						Projectile.NewProjectile(Xpos - 20, Ypos + -(2 * (Ypos - Ycen)) + 20, -5, +5, mod.ProjectileType("PlanetaryFlame2"), Damage, 0, player.whoAmI);
					}
				}
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PlanetaryCore", 1);
			recipe.AddIngredient(null, "EmptyPlanetariumOrb", 20);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
