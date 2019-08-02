using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class DropAmmo : ModBuff
    {
        public override void SetDefaults()
        {
           DisplayName.SetDefault("Drop Ammo");
			Description.SetDefault("");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
		
        }
	}
}
