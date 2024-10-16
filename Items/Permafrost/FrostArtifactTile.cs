using Microsoft.Xna.Framework;
using SOTS.Buffs;
using SOTS.Helpers;
using SOTS.Items.Pyramid;
using SOTS.NPCs.Boss.Polaris;
using SOTS.NPCs.Boss.Polaris.NewPolaris;
using SOTS.Void;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.UI.ModBrowser;
using Terraria.ObjectData;

namespace SOTS.Items.Permafrost
{
	public class FrostArtifactTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileObsidianKill[Type] = false;
			Main.tileSolid[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = false;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 36;
			TileObjectData.addTile(Type);
			DustType = DustID.Ice; 
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(135, 150, 170), name);
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 1200;
		}
		public override void NearbyEffects(int i, int j, bool closer) 
		{
			if(closer && Main.tile[i, j + 1].TileType == ModContent.TileType<FrostArtifactTile>() && Main.tile[i - 1, j + 1].TileType == ModContent.TileType<FrostArtifactTile>() && Main.tile[i + 1, j + 1].TileType == ModContent.TileType<FrostArtifactTile>())
			{
				int xlocation = i * 16 + 8;
				int ylocation = j * 16 + 8;
				ylocation -= 40;
				for(int k = 0; k < 5; k++)
				{
					float counter = ColorHelper.SoulColorCounter * 2f + k;
					for (int a = 0; a < 4; a++)
					{
						Vector2 circularLocation = new Vector2(-28, 0).RotatedBy(MathHelper.ToRadians(counter + a * 90));
						Dust dust = Dust.NewDustDirect(new Vector2(xlocation + circularLocation.X - 4, ylocation + circularLocation.Y - 4), 4, 4, 135); //ice torch
						dust.noGravity = true;
						dust.velocity *= 0.1f;
						dust.scale = dust.scale * 0.3f + 1.2f;
					}
				}
			}
		}
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			return SOTSWorld.downedAmalgamation;
		}
        public override bool RightClick(int i, int j)
        {
			Main.mouseRightRelease = true;
            Player player = Main.LocalPlayer;
			if(player.HasItem(ModContent.ItemType<FrostedKey>()))
            {
                if (!NPC.AnyNPCs(ModContent.NPCType<Polaris>()) && !NPC.AnyNPCs(ModContent.NPCType<NewPolaris>()))
                {
                    Projectile.NewProjectile(player.GetSource_TileInteraction(i, j), player.Center + new Vector2(0, -1000), Vector2.Zero, ModContent.ProjectileType<SpawnEnemyProj>(), 0, 0, Main.myPlayer, -2);
                }
            }
            return true;
		}  
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			offsetY = 0;
		}
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			//Tile tile = Main.tile[i, j];
			//int left = i;
			//int top = j;
			player.cursorItemIconID = ModContent.ItemType<FrostedKey>();
			//player.cursorItemIconText = "";
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
		}
		public override void MouseOverFar(int i, int j)
		{
			MouseOver(i, j);
			Player player = Main.LocalPlayer;
			if (player.cursorItemIconText == "")
			{
				player.cursorItemIconEnabled = false;
				player.cursorItemIconID = 0;
			}
		}
	}
}