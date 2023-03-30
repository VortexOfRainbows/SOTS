using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;
using SOTS.Void;

namespace SOTS.Items.Secrets
{
	public class ForgottenLampTile : ModTile
    {
        public override void SetStaticDefaults()
        {
		    MineResist = 0.01f;
		    MinPick = 0;
            Main.tileNoFail[Type] = true;
            Main.tileSolid[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 36;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.Table, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);
            DustType = DustID.Cloud;
            ModTranslation name = CreateMapEntryName();
		    AddMapEntry(new Color(255, 255, 239), name);
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            return false;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
             if(frameX == 0)
             {
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 32, ModContent.ItemType<DreamLamp>());
             }
        }
	    public override bool CanExplode(int i, int j)
		{
			return true;
		}
    	public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			offsetY = 2;
        }
    }
    public class ForgottenLampProjectile : ModProjectile
    {
        public override string Texture => "SOTS/Items/Secrets/DreamLamp";
        public override void SetDefaults()
        {
            Projectile.friendly = false;
            Projectile.hide = false;
            Projectile.timeLeft = 150;
            Projectile.width = 16;
            Projectile.height = 16;
        }
        float alphaMult = 0;
        public override bool PreAI()
        {
            bool DoesProjectileExist = false;
            for (int i = 0; i < 1000; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.type == ModContent.ProjectileType<ForgottenLampProjectile>() && Projectile.whoAmI != i)
                {
                    DoesProjectileExist = true;
                    break;
                }
            }
            if (DoesProjectileExist)
                Projectile.Kill();
            return true;
        }
        public override void AI()
        {
            int i = (int)(Projectile.Center.X / 16);
            int j = (int)(Projectile.Center.Y / 16);
            Tile tile = Framing.GetTileSafely(i, j);
            if(tile.HasUnactuatedTile && tile.TileType == ModContent.TileType<ForgottenLampTile>())
            {
                if(alphaMult < 1)
                   alphaMult += 1 / 90f;
            }
            else
            {
                Projectile.Kill();
            }
        }
        public override void Kill(int timeLeft)
        {
            if (timeLeft > 10)
                return;
            int i = (int)(Projectile.Center.X / 16);
            int j = (int)(Projectile.Center.Y / 16);
            Tile tile = Framing.GetTileSafely(i, j);
            if (tile.HasUnactuatedTile && tile.TileType == ModContent.TileType<ForgottenLampTile>())
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    WorldGen.KillTile(i, j, false, false, false);
                    SOTSWorld.DreamLampSolved = true;
                    if(Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j);
                        SOTSWorld.SyncGemLocks(null);
                    }
                }
                if (Main.netMode != NetmodeID.Server)
                {
                    SOTSUtils.PlaySound(SoundID.Item30, Projectile.Center, 1f, -0.1f);
                    for (int a = 0; a < 64; a++)
                    {
                        Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<Dusts.AlphaDrainDust>(), 0, 0, 0, VoidPlayer.natureColor, 1.1f);
                        dust.scale *= 1.4f;
                        dust.velocity *= 0.6f;
                        dust.velocity += new Vector2(0, 9f).RotatedBy(MathHelper.TwoPi * a / 24f) / dust.scale;
                        dust.fadeIn = 0.1f;
                        dust.noGravity = true;
                    }
                    //SOTSWorld.SecretFoundMusicTimer = 720;
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            ConduitHelper.DrawConduitCircleFull(Projectile.Center, 1 - (Projectile.timeLeft / 150f), VoidPlayer.natureColor * alphaMult);
            return false;
        }
    }
}