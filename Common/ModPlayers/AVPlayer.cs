using Microsoft.Xna.Framework;
using SOTS.Buffs.Debuffs;
using SOTS.Dusts;
using SOTS.Helpers;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Common.ModPlayers
{
    public class AVPlayer : ModPlayer
    {
        public bool InVillage => Player.SOTSPlayer().AbandonedVillageBiome;
        public bool InUnderground => Player.ZoneRockLayerHeight || Player.ZoneDirtLayerHeight;
        private bool Moving => Player.velocity.X > 1 || Player.velocity.Y > 1 || Player.velocity.X < -1 || Player.velocity.Y < -1; //Basically, am I moving a sufficient amount?
        public override void PostUpdateMiscEffects()
        {
            if (Player.HasBuff<LungRot>())
            {
                if (Moving)
                {
                    Player.lifeRegen = (int)(Player.lifeRegen * LungRot.LifeRegenWhileMoving);
                    Player.VoidPlayer().voidRegenSpeed += 0.4f;
                }
            }
            if (InVillage)
            {
                Player.AddBuff(ModContent.BuffType<LungRot>(), 6, true);
                if (Main.myPlayer == Player.whoAmI)
                    SpawnAmbientParticles();
            }
        }
        public override void NaturalLifeRegen(ref float regen)
        {
            if (Player.HasBuff<LungRot>())
            {
                if (Moving)
                {
                    regen *= LungRot.LifeRegenWhileMoving;
                }
            }
        }
        private void SpawnAmbientParticles()
        {
            //This is where dust spawning will go
            float w = Main.screenWidth;
            float h = Main.screenHeight;
            Vector2 center = Main.screenPosition + new Vector2(w * 0.5f, h * 0.5f);
            float targetParticleCount = 10 / Main.GameZoomTarget;
            Vector2 windDirection = new Vector2(Main.windSpeedCurrent * (InUnderground ? 0.2f : 1f), 0) * 2f;
            for(int a = 0; a < targetParticleCount; a++)
            {
                Vector2 position = center + new Vector2(w * 0.5f * Main.rand.NextFloat(-1, 1f), h * 0.5f * Main.rand.NextFloat(-1, 1f)) / Main.GameZoomTarget;
                int i = (int)position.X / 16;
                int j = (int)position.Y / 16;
                Vector2? dustPosition = SOTSTile.GetWorldPositionOnTile(i, j, Main.rand.Next(4), position.X - i * 16, position.Y - j * 16, true);
                if (dustPosition != null)
                {
                    Vector2 awayFromBlockSurface = (dustPosition.Value - position).SNormalize() * Main.rand.NextFloat(1f);
                    PixelDust.Spawn(dustPosition.Value, 0, 0, windDirection + Main.rand.NextVector2Circular(0.5f, 0.5f) + awayFromBlockSurface, ColorHelper.AVDustColor * Main.rand.NextFloat(0.65f, 0.85f), 2).scale = 1;

                }
            }
        }
    }
}