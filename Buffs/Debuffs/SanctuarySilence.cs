using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.ModLoader;
 
namespace SOTS.Buffs.Debuffs
{
    public class SanctuarySilence : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
			Main.debuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
		{

        }
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            buffName = Language.GetText("Mods.SOTS.Buffs.SanctuarySilence.Prefix" + Main.moonPhase) + buffName;
            string s = (SOTSWorld.MoonPhasePercent * 100).ToString("##.#");
            tip = Language.GetTextValue("Mods.SOTS.Buffs.SanctuarySilence.Description", s);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, int buffIndex, ref BuffDrawParams drawParams)
        {
            return true;
        }
        public override void PostDraw(SpriteBatch spriteBatch, int buffIndex, BuffDrawParams drawParams)
        {
            Player player = Main.LocalPlayer; //player is always local for buff drawing
            float sin = MathF.Sin(SOTSWorld.MoonPhasePercent * MathF.PI);
            int x = 8;
            int w = 16;
            float time = Utils.GetDayTimeAs24FloatStartingFromMidnight() % 24;
            if (Main.moonPhase >= 4 || (Main.moonPhase == 0 && time > 4.5f)) //New/full moon and waxing
            {
                w = (int)(w * SOTSWorld.MoonPhasePercent + 0.5f);
                x += 16 - w;
            }
            else // waning and some full moon
            {
                w = (int)(w * SOTSWorld.MoonPhasePercent + 0.5f);
            }
            Rectangle frame = new Rectangle(x, 8, w, 16);
            float moonDist = .4f + 1.8f * SOTSWorld.MoonPhasePercent;
            Color c = drawParams.DrawColor;
            c.A = 0;
            for (int i = 0; i < 6; i++)
            {
                Vector2 circular = new Vector2(moonDist * 1.5f, 0).RotatedBy(MathHelper.ToRadians(SOTSWorld.GlobalCounter + i * 60));
                spriteBatch.Draw(ModContent.Request<Texture2D>("SOTS/Buffs/Debuffs/SanctuarySilenceFull").Value, drawParams.Position + new Vector2(x, 8) + circular, frame, c * 0.23f * SOTSWorld.MoonPhasePercent, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            spriteBatch.Draw(ModContent.Request<Texture2D>("SOTS/Buffs/Debuffs/SanctuarySilenceFull").Value, drawParams.Position + new Vector2(x, 8), frame, drawParams.DrawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}