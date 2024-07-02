using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using SOTS.Common.ModPlayers;

namespace SOTS.Buffs.ConduitBoosts
{
    public class Attuned : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.persistentBuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {

        }
        public override bool RightClick(int buffIndex)
        {
            Player player = Main.LocalPlayer;
            ConduitPlayer CP = player.ConduitPlayer();
            CP.NaturePower = CP.EarthPower = CP.PermafrostPower = CP.OtherworldPower = CP.TidePower = CP.EvilPower = CP.InfernoPower = CP.ChaosPower = 0;
            return true;
        }
        public override void PostDraw(SpriteBatch spriteBatch, int buffIndex, BuffDrawParams drawParams)
        {
            Texture2D texture = ModContent.Request<Texture2D>("SOTS/Buffs/ConduitBoosts/AttunedTiers").Value;
            Rectangle frame = new Rectangle(0, 0, texture.Width, texture.Height / 8);
            spriteBatch.Draw(texture, drawParams.Position, frame, drawParams.DrawColor, 0f, Vector2.Zero, 1f, 0, 0);
        }
    }
}