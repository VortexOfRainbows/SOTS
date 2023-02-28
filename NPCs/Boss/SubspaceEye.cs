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
            Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
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
            NPC.value = 0;
            NPC.npcSlots = 1f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit3;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.netAlways = true;
            NPC.hide = true;
            NPC.dontCountMe = true;
            NPC.alpha = 255;
            NPC.dontTakeDamage = true;
        }
        public override void DrawBehind(int index)
        {
            Main.instance.DrawCacheNPCProjectiles.Add(index);
        }
        int frame = 0;
        public float eyeRecoil = 1;
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if(NPC.frameCounter > 4)
            {
                NPC.frameCounter = 0;
                frame++;
                if(frame >= 8)
                {
                    frame = 0;
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/SubspaceEyeDraw");
            Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/SubspaceEyeFlames");
            Texture2D texture3 = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/SubspaceEyePupil");
            Vector2 origin = new Vector2(texture.Width / 2, 102);
            for (int a = 0; a < 360; a += 30)
            {
                Vector2 circular = new Vector2(Main.rand.NextFloat(2.5f, 5f), 0).RotatedBy(MathHelper.ToRadians(a));
                Color color = new Color(100, 255, 100, 0);
                spriteBatch.Draw(texture2, NPC.Center + circular - screenPos, new Rectangle(0, frame * 120, texture.Width, 120), color * ((255f - NPC.alpha) / 255f) * 0.5f, NPC.rotation, origin, 1.00f, SpriteEffects.None, 0.0f);
            }
            spriteBatch.Draw(texture, NPC.Center - screenPos, new Rectangle(0, frame * 120, texture.Width, 120), new Color(65, 155, 65) * ((255f - NPC.alpha) / 255f), NPC.rotation, origin, 1.00f, SpriteEffects.None, 0.0f);
            Player player = Main.player[Main.myPlayer];
            Vector2 toPlayer = player.Center - NPC.Center;
            Vector2 eyeOffset = new Vector2(8 * eyeRecoil, 0).RotatedBy(toPlayer.ToRotation());
            eyeOffset.Y *= 0.5f;
            spriteBatch.Draw(texture3, NPC.Center + eyeOffset - screenPos, new Rectangle(0, frame * 120, texture.Width, 120), new Color(65, 155, 65) * ((255f - NPC.alpha) / 255f), NPC.rotation, origin, 1.00f, SpriteEffects.None, 0.0f);
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
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Celestial.SubspaceEye>(), 0, 0, Main.myPlayer, NPC.whoAmI);
                runOnce = false;
            }
            if(NPC.ai[3] == -1)
            {
                if (NPC.alpha < 255)
                    NPC.alpha++;
                else
                    NPC.active = false;
            }
            else if (NPC.alpha > 0)
                NPC.alpha--;
            else
                NPC.dontTakeDamage = true;
        }
	}
}





















