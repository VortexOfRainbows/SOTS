using System;
using Terraria;
using Terraria.ModLoader;
 
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace SOTS.Buffs
{
    public class Catalyst : ModBuff
    { int regenTimer = 0;
        public override void SetDefaults()
        {
           DisplayName.SetDefault("Catalyst");
			Description.SetDefault("They're growing off you!");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
			Main.debuff[Type] = true;
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
			regenTimer += 1;
			Rectangle rect = player.getRect();
                Dust.NewDust(new Vector2(rect.X, rect.Y), rect.Width, rect.Height, 75);
				Dust.NewDust(new Vector2(rect.X, rect.Y), rect.Width, rect.Height, 75);
				
				
			
			
			if(regenTimer == 24)
			{
				Projectile.NewProjectile((player.Center.X), player.Center.Y, Main.rand.Next(-8, 9), Main.rand.Next(-8, 9), mod.ProjectileType("SightEssence"), 33, 0, player.whoAmI);
				Projectile.NewProjectile((player.Center.X), player.Center.Y, Main.rand.Next(-8, 9), Main.rand.Next(-8, 9), mod.ProjectileType("SightEssence"), 33, 0, player.whoAmI);
				Projectile.NewProjectile((player.Center.X), player.Center.Y, Main.rand.Next(-8, 9), Main.rand.Next(-8, 9), mod.ProjectileType("SightEssence"), 33, 0, player.whoAmI);
				

			regenTimer = 0;
			}
        }
    }
}