using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Boss
{
	public class SubspaceEye : ModNPC
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Subspace Gaze");
            Main.npcFrameCount[NPC.type] = 1;
        }
		public override void SetDefaults()
		{
            NPC.aiStyle =0; 
            NPC.lifeMax = 40000;   
            NPC.damage = 0; 
            NPC.defense = 30;  
            NPC.knockBackResist = 0f;
            NPC.width = 68;
            NPC.height = 60;
            npc.value = 0;
            npc.npcSlots = 1f;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit3;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.netAlways = true;
            npc.buffImmune[44] = true;
            npc.hide = true;
            npc.dontCountMe = true;
            npc.alpha = 255;
            npc.dontTakeDamage = true;
        }
        public override void DrawBehind(int index)
        {
            Main.instance.DrawCacheNPCProjectiles.Add(index);
        }
        int frame = 0;
        public float eyeRecoil = 1;
        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if(npc.frameCounter > 4)
            {
                npc.frameCounter = 0;
                frame++;
                if(frame >= 8)
                {
                    frame = 0;
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/SubspaceEyeDraw");
            Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/SubspaceEyeFlames");
            Texture2D texture3 = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/SubspaceEyePupil");
            Vector2 origin = new Vector2(texture.Width / 2, 102);
            for (int a = 0; a < 360; a += 30)
            {
                Vector2 circular = new Vector2(Main.rand.NextFloat(2.5f, 5f), 0).RotatedBy(MathHelper.ToRadians(a));
                Color color = new Color(100, 255, 100, 0);
                Main.spriteBatch.Draw(texture2, npc.Center + circular - Main.screenPosition, new Rectangle(0, frame * 120, texture.Width, 120), color * ((255f - npc.alpha) / 255f) * 0.5f, npc.rotation, origin, 1.00f, SpriteEffects.None, 0.0f);
            }
            Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition, new Rectangle(0, frame * 120, texture.Width, 120), new Color(65, 155, 65) * ((255f - npc.alpha) / 255f), npc.rotation, origin, 1.00f, SpriteEffects.None, 0.0f);
            Player player = Main.player[Main.myPlayer];
            Vector2 toPlayer = player.Center - npc.Center;
            Vector2 eyeOffset = new Vector2(8 * eyeRecoil, 0).RotatedBy(toPlayer.ToRotation());
            eyeOffset.Y *= 0.5f;
            Main.spriteBatch.Draw(texture3, npc.Center + eyeOffset - Main.screenPosition, new Rectangle(0, frame * 120, texture.Width, 120), new Color(65, 155, 65) * ((255f - npc.alpha) / 255f), npc.rotation, origin, 1.00f, SpriteEffects.None, 0.0f);
            return false;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return true;       //this make that the npc does not have a health bar
        }
        bool runOnce = true;
        public override void AI()
        {
            if (eyeRecoil < 1)
            {
                eyeRecoil += 0.04f;
            }
            else
            {
                eyeRecoil = 1;
            }
            if(runOnce)
            {
                if (Main.netMode != 1)
                    Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Celestial.SubspaceEye>(), 0, 0, Main.myPlayer, npc.whoAmI);
                runOnce = false;
            }
            if(npc.ai[3] == -1)
            {
                if (npc.alpha < 255)
                    npc.alpha++;
                else
                    npc.active = false;
            }
            else if (npc.alpha > 0)
                npc.alpha--;
            else
                npc.dontTakeDamage = true;
        }
	}
}





















