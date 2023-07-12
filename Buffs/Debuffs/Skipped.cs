using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Camera;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Buffs.Debuffs
{
    public class Skipped : ModBuff
    {	
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
        }
    }
}