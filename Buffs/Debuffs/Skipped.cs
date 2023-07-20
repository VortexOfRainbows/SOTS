using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.NPCs.Town;
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
        public override void Update(Player p, ref int buffIndex)
        {
            p.noFallDmg = true;
            if (p.buffTime[buffIndex] == 296)
                PortalDrawingHelper.DustCircle(p.Center, p.width * 0.5f, p.height * 0.5f);
        }
    }
}