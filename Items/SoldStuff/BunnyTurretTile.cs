using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
namespace SOTS.Items.SoldStuff
{
	public class BunnyTurretTile : ModTile
	{	
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Bunny Turret");
			AddMapEntry(new Color(235, 235, 235), name);
			dustType = 30;
			disableSmartCursor = true;
		}
		private readonly int animationFrameWidth = 36;
	     //int fireRate = 60;
		 int frame = 0;
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 32, 32, mod.ItemType("BunnyTurret"));
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch) 
		{
			//fireRate++;
			return true;
		}
		public override void HitWire(int i, int j)
		{
				float left = i * 16;
				float top = j * 16;
				left += 16.5f;
				top += 16.5f;
				   float shootToX = 10;
				   float shootToY = -1;
				   if(frame == 1)
				   {
					   shootToX = -10;
				   } 
				   if(frame == 2)
				   {
					   shootToX = 1;
					   shootToY = -10;
				   }
				   if(frame == 3)
				   {
					   shootToX = -1;
					   shootToY = -10;
				   }
			for(int k = 0; k < 200; k++)
			{
				
				   NPC target = Main.npc[k];
					
				   shootToX = target.position.X + (float)target.width * 0.5f - left;
				   shootToY = target.position.Y + (float)target.height * 0.5f - top;
				   float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
					
					
				   if(distance < 640f && !target.friendly && target.active)
				   {
						frame = 0;
					  if (target.Center.X < left)
						{
						frame++;
						}
						if (target.Center.Y < top - 128)
						{
						frame += 2;
						}
					 
						   distance = 3f / distance;
			   
						   shootToX *= distance * 5;
						   shootToY *= distance * 5;
							//Spawning a projectile
							//fireRate = 0;
							break;
					   
				   }
			}
			Projectile.NewProjectile(left, top, shootToX, shootToY, 281, 35, 0, Main.myPlayer, 0f, 0f);
		
		}public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset) 
		{
		
			int uniqueAnimationFrame = Main.tileFrame[Type];
			
				uniqueAnimationFrame += frame;
				
			frameXOffset = uniqueAnimationFrame * animationFrameWidth;
		}

	} 
}