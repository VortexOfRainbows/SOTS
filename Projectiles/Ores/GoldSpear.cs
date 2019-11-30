using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Ores
{    
    public class GoldSpear : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gold Spear");
			
		}
        public override void SetDefaults()
        {
            projectile.CloneDefaults(367); //obsidian swordfish
            aiType = 367;
            projectile.melee = true;
			projectile.alpha = 0; 
		}
	}
}