using System;
using Terraria;
using Terraria.ModLoader;
 
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace SOTS.Buffs
{
    public class Relocated : ModBuff
    { int regenTimer = 0;
        public override void SetDefaults()
        {
           DisplayName.SetDefault("Relocated");
			Description.SetDefault("You just got warped");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
			regenTimer += 1;
			Rectangle rect = player.getRect();
                Dust.NewDust(new Vector2(rect.X, rect.Y), rect.Width, rect.Height, 63);
				Dust.NewDust(new Vector2(rect.X, rect.Y), rect.Width, rect.Height, 204);
				
				
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			
			
			modPlayer.SpectreCool = true;
			if(regenTimer == 24)
			{
				Projectile.NewProjectile((player.Center.X), player.Center.Y, Main.rand.Next(-8, 9), Main.rand.Next(-8, 9), mod.ProjectileType("PatronusShot"), 33, 0, player.whoAmI);
				Projectile.NewProjectile((player.Center.X), player.Center.Y, Main.rand.Next(-8, 9), Main.rand.Next(-8, 9), mod.ProjectileType("PatronusShot"), 33, 0, player.whoAmI);
				Projectile.NewProjectile((player.Center.X), player.Center.Y, Main.rand.Next(-8, 9), Main.rand.Next(-8, 9), mod.ProjectileType("PatronusShot"), 33, 0, player.whoAmI);
				
				Projectile.NewProjectile((player.Center.X), player.Center.Y, Main.rand.Next(-8, 9), Main.rand.Next(-8, 9), mod.ProjectileType("PatronusShot"), 33, 0, player.whoAmI);
				Projectile.NewProjectile((player.Center.X), player.Center.Y, Main.rand.Next(-8, 9), Main.rand.Next(-8, 9), mod.ProjectileType("PatronusShot"), 33, 0, player.whoAmI);
				Projectile.NewProjectile((player.Center.X), player.Center.Y, Main.rand.Next(-8, 9), Main.rand.Next(-8, 9), mod.ProjectileType("PatronusShot"), 33, 0, player.whoAmI);

			regenTimer = 0;
			}
        }
    }
}