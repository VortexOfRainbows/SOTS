using System;
using Terraria;
using Terraria.ModLoader;
 
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace SOTS.Buffs
{
    public class HoneyFinned : ModBuff
    { int regenTimer = 0;
        public override void SetDefaults()
        {
           DisplayName.SetDefault("Honey Finned");
			Description.SetDefault("Additional healing");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
			regenTimer--;
			if(regenTimer <= 0)
			{
			player.statLife += 30;
			player.HealEffect(30);
			regenTimer = 900;
			}
        }
    }
}