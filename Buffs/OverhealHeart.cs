using System;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs
{
    public class OverhealHeart : ModBuff
    {
        public override void SetStaticDefaults()
        {
           DisplayName.SetDefault("Drop Heart");
			Description.SetDefault("");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
		
        }
		public override void Update(NPC target, ref int buffIndex)
		{
			
				Player player  = Main.player[target.target];
			if(target.type == Mod.Find<ModNPC>("Libra").Type)
			{
				if(player.name == "Xypher" || player.name == "X")
				{
					Main.NewText("Trying out Xypher this time huh?", 255, 255, 255);
				}
				else
				{
					Main.NewText("Luck, that was all just luck.", 255, 255, 255);
					
				}
					Item.NewItem((int)target.position.X, (int)target.position.Y, target.width, target.height, (Mod.Find<ModItem>("ManaMaterial").Type), 1);
					target.timeLeft = 1;
					target.timeLeft--;
					target.life = 0;
					
			}
}
    }
}