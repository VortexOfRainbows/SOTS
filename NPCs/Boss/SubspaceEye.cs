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
            Main.npcFrameCount[npc.type] = 1;
        }
		public override void SetDefaults()
		{
            npc.aiStyle = 0; 
            npc.lifeMax = 40000;   
            npc.damage = 0; 
            npc.defense = 30;  
            npc.knockBackResist = 0f;
            npc.width = 68;
            npc.height = 60;
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
            npc.alpha = 255;
		}
        public override void DrawBehind(int index)
        {
            Main.instance.DrawCacheNPCProjectiles.Add(index);
        }
        int frame = 0;
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
            Texture2D texture = ModContent.GetTexture("SOTS/NPCs/Boss/SubspaceEyeDraw");
            Texture2D texture2 = ModContent.GetTexture("SOTS/NPCs/Boss/SubspaceEyeFlames");
            Vector2 origin = new Vector2(texture.Width / 2, 84);
            for (int a = 0; a < 360; a += 30)
            {
                Vector2 circular = new Vector2(Main.rand.NextFloat(2.5f, 5f), 0).RotatedBy(MathHelper.ToRadians(a));
                Color color = new Color(100, 255, 100, 0);
                Main.spriteBatch.Draw(texture2, npc.Center + circular - Main.screenPosition, new Rectangle(0, frame * 120, texture.Width, 120), color * ((255f - npc.alpha) / 255f) * 0.5f, npc.rotation, origin, 1.00f, SpriteEffects.None, 0.0f);
            }
            Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition, new Rectangle(0, frame * 120, texture.Width, 120), new Color(65, 155, 65) * ((255f - npc.alpha) / 255f), npc.rotation, origin, 1.00f, SpriteEffects.None, 0.0f);
            return false;
        }
        bool runOnce = true;
        public override void AI()
        {
            if(runOnce)
            {
                if (Main.netMode != 1)
                    Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Celestial.SubspaceEye>(), 0, 0, Main.myPlayer, npc.whoAmI);
                runOnce = false;
            }
            if (npc.alpha > 0)
                npc.alpha--;
        }
	}
}





















