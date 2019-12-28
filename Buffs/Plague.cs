using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
 
namespace SOTS.Buffs
{
    public class Plague : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Plague");
			Description.SetDefault("'It basically just spreads poison'");   
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
        }
		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.lifeRegen -= 1;
			if(!npc.buffImmune[20] && !npc.buffImmune[24])
			{
				npc.lifeRegen -= 3;
				if(npc.boss)
				{
					npc.lifeRegen -= 3;
					for(int i = 1; npc.lifeMax >= 12 * i; i *= 2)
					{
						npc.lifeRegen -= 1;
					}
				}
				else
				{
					for(int i = 1; npc.lifeMax >= 8 * i; i *= 2)
					{
						npc.lifeRegen -= 1;
					}
				}
			}
			if(npc.buffTime[buffIndex] % 6 == 0)
			{
				for(int i = 0; i < 360; i += 60)
				{
					Vector2 circularLocation = new Vector2(npc.width/2 - 10, 0).RotatedBy(MathHelper.ToRadians(i));
					int num1 = Dust.NewDust(new Vector2(npc.Center.X + circularLocation.X - 4, npc.Center.Y + circularLocation.Y - 4), 4, 4, 38);
					Main.dust[num1].noGravity = true;
					circularLocation.Normalize();
					Main.dust[num1].velocity = circularLocation * 5.5f;
				}
			}
			for(int i = 0; i < 200; i++) //find first enemy
			{
				NPC npc2 = Main.npc[i];
				if(!npc2.friendly && npc2.lifeMax > 1 && npc2.active && !npc2.HasBuff(mod.BuffType("Plague")) && npc.buffTime[buffIndex] > 120)
				{
					float disX = npc.Center.X - npc2.Center.X;
					float disY = npc.Center.Y - npc2.Center.Y;
					double dis = Math.Sqrt(disX * disX + disY * disY);
					if(dis < 164)
					{
						if(npc.buffTime[buffIndex] % 7 == 0 || npc.buffTime[buffIndex] == 121)
						{
							npc2.AddBuff(mod.BuffType("Plague"), npc.buffTime[buffIndex] + 6, false);
							for(int j = 0; j < 360; j += 10)
							{
								Vector2 circularLocation = new Vector2(npc2.width/2 - 12, 0).RotatedBy(MathHelper.ToRadians(j));
								int num1 = Dust.NewDust(new Vector2(npc2.Center.X + circularLocation.X - 4, npc2.Center.Y + circularLocation.Y - 4), 4, 4, 36);
								Main.dust[num1].noGravity = true;
								Main.dust[num1].scale = 1.65f;
								circularLocation.Normalize(); 
								Main.dust[num1].velocity = circularLocation * 12.5f;
							}
						}
					}
				}
			}
		}
    }
}