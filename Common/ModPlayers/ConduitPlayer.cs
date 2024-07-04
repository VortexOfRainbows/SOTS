using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using SOTS.Buffs.ConduitBoosts;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static SOTS.SOTS;

namespace SOTS.Common.ModPlayers
{
    public class ConduitPlayer : ModPlayer
    {
        public void SendAllPacket(int toClient, int fromClient)
        {
            var packet = Mod.GetPacket();
            packet.Write((byte)SOTSMessageType.SyncConduitPlayerAll);
            packet.Write((byte)Player.whoAmI);
            packet.Write(NaturePower);
            packet.Write(EarthPower);
            packet.Write(PermafrostPower);
            packet.Write(OtherworldPower);
            packet.Write(TidePower);
            packet.Write(EvilPower);
            packet.Write(InfernoPower);
            packet.Write(ChaosPower);
            packet.Send(toClient, fromClient);
        }
        private void SendPacket(int type, int value, int toClient, int fromClient)
        {
            var packet = Mod.GetPacket();
            packet.Write((byte)SOTSMessageType.SyncConduitPlayer);
            packet.Write((byte)Player.whoAmI);
            packet.Write(type);
            packet.Write(value);
            packet.Send(toClient, fromClient);
        }
        public override void SaveData(TagCompound tag)
        {
            tag["Power1"] = NaturePower;
            tag["Power2"] = EarthPower;
            tag["Power3"] = PermafrostPower;
            tag["Power4"] = OtherworldPower;
            tag["Power5"] = TidePower;
            tag["Power6"] = EvilPower;
            tag["Power7"] = InfernoPower;
            tag["Power8"] = ChaosPower;
        }
        public override void LoadData(TagCompound tag)
        {
            NaturePower = tag.GetInt("Power1");
            EarthPower = tag.GetInt("Power2");
            PermafrostPower = tag.GetInt("Power3");
            OtherworldPower = tag.GetInt("Power4");
            TidePower = tag.GetInt("Power5");
            EvilPower = tag.GetInt("Power6");
            InfernoPower = tag.GetInt("Power7");
            ChaosPower = tag.GetInt("Power8");
        }
        public const float ChargeTime = 90f;
        public static ConduitPlayer ModPlayer(Player player)
        {
            return player.GetModPlayer<ConduitPlayer>();
        }
        public bool NatureBoosted => NaturePower >= ChargeTime;
        public bool EarthBoosted => EarthPower >= ChargeTime;
        public bool PermafrostBoosted => PermafrostPower >= ChargeTime;
        public bool OtherworldBoosted => OtherworldPower >= ChargeTime;
        public bool TideBoosted => TidePower >= ChargeTime;
        public bool EvilBoosted => EvilPower >= ChargeTime;
        public bool InfernoBoosted => InfernoPower >= ChargeTime;
        public bool ChaosBoosted => ChaosPower >= ChargeTime;
        public int NaturePower = 0;
        public int EarthPower = 0;
        public int PermafrostPower = 0;
        public int OtherworldPower = 0;
        public int TidePower = 0;
        public int EvilPower = 0;
        public int InfernoPower = 0;
        public int ChaosPower = 0;
        public override void ResetEffects()
        {
            IteratePower(ref NaturePower, ColorHelpers.NatureColor, 0);
            IteratePower(ref EarthPower, ColorHelpers.EarthColor, 1);
            IteratePower(ref PermafrostPower, ColorHelpers.PermafrostColor, 2);
            IteratePower(ref OtherworldPower, ColorHelpers.PurpleOtherworldColor, 3);
            IteratePower(ref TidePower, ColorHelpers.TideColor, 4);
            IteratePower(ref EvilPower, ColorHelpers.RedEvilColor, 5);
            IteratePower(ref InfernoPower, ColorHelpers.Inferno1, 6);
            IteratePower(ref ChaosPower, ColorHelpers.ChaosPink, 7);

            if(NatureBoosted || EarthBoosted || PermafrostBoosted || OtherworldBoosted || TideBoosted || EvilBoosted || InfernoBoosted || ChaosBoosted)
            {
                Player.AddBuff(ModContent.BuffType<Attuned>(), 300);
            }
        }
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            SendAllPacket(toWho, fromWho);
        }
        private void IteratePower(ref int Power, Color color, int num)
        {
            if(Power > 0)
            {
                if (Power < ChargeTime)
                {
                    if(Main.netMode == NetmodeID.MultiplayerClient && Main.myPlayer == Player.whoAmI)
                    {
                        if(Power <= 5)
                            SendPacket(num, Power, -1, Player.whoAmI);
                    }
                    Power++;
                    if(Power >= ChargeTime)
                    {
                        SOTSUtils.PlaySound(SoundID.Item30, Player.Center, 0.75f, 0.25f);
                        for (int i = 0; i < 24; i++)
                        {
                            Dust dust = Dust.NewDustDirect(Player.Center, 0, 0, ModContent.DustType<Dusts.AlphaDrainDust>(), 0, 0, 0, color, 1.1f);
                            dust.scale *= 1.4f;
                            dust.velocity *= 0.6f;
                            dust.velocity += new Vector2(0, 9f).RotatedBy(MathHelper.TwoPi * i / 24f) / dust.scale;
                            dust.fadeIn = 0.1f;
                            dust.noGravity = true;
                        }
                    }
                }
                else
                {
                    Power = (int)ChargeTime;
                }
            }
        }
    }
}