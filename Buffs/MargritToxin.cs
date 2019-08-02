using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SOTS.Buffs
{
    public class MargritToxin : ModBuff
    {
        public override void SetDefaults()
        {
           DisplayName.SetDefault("Margrit Toxin");
			Description.SetDefault("");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
		
        }
		public override void Update(NPC target, ref int buffIndex)
		{
			for(int i = 0; i < 5; i++)
			{
			int num1 = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, 197);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			}
			
				Player player  = Main.player[target.target];
			
			}
	}
}