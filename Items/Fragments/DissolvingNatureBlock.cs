using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.Fragments
{
	public class DissolvingNatureBlock : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nature Block");
			Tooltip.SetDefault("");
		}

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = 3;
			item.consumable = true;
			item.createTile = mod.TileType("DissolvingNatureTile");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "DissolvingNature", 1);
			recipe.SetResult(this, 20);
			recipe.AddRecipe();
		}
	}
	public class DissolvingNatureTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileShine2[Type] = true;
			Main.tileLighted[Type] = true;
			drop = mod.ItemType("DissolvingNatureBlock");
			AddMapEntry(new Color(177, 238, 181));
			mineResist = 0.2f;
            //soundType = 21;
            //soundStyle = 2;
			dustType = 32;
			TileID.Sets.GemsparkFramingTypes[Type] = Type;
		} 
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 1.7f;
			g = 2.3f;
			b = 1.8f;
			r *= 0.3f;
			g *= 0.3f;
			b *= 0.3f;
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch) 
		{
			Texture2D texture = mod.GetTexture("Gores/NatureParticle");
			Texture2D textureBlock = mod.GetTexture("Gores/NatureBlockOutline");
			Color color = new Color(100, 100, 100, 0);
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if(Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			for(int k = 0; k < 16; k += 2)
			{
				Vector2 location = new Vector2(i * 16, j * 16);
				color = new Color(100, 100, 100, 0);
				int seed = k + j + (i * j); 
				int uniqueParticleFrame = i - seed + (int)(Main.GlobalTime * 5); //serves as a type of randomizer for the particles
				if(i % 2 == 0)
					uniqueParticleFrame += 3;
				if(i % 3 == 0)
					uniqueParticleFrame += 3;
				if(i % 4 == 0)
					uniqueParticleFrame += 3;
				if(seed % 2 == 0)
					uniqueParticleFrame += 2;
				if(seed % 3 == 0)
					uniqueParticleFrame += 4;
				if(seed % 6 == 0)
					uniqueParticleFrame += 3;
				if(seed % 7 == 0)
					uniqueParticleFrame += 3; 
				
				uniqueParticleFrame = uniqueParticleFrame % (11 + (((k/2) % 2 == 0) ? 1 : 0)+ ((((k/2) + i) % 3 == 0) ? 1 : 0));
				if(uniqueParticleFrame < 8)
				location.Y -= uniqueParticleFrame * 2;
				if(Main.tile[i, j].halfBrick() == true)
				location.Y += 8;
				if(Main.tile[i, j].slope() == 1)
				location.Y += k;
				if(Main.tile[i, j].slope() == 2)
				location.Y += 15 - k;
					
				location.X += k;
				Vector2 drawPos = location - Main.screenPosition;
				if(uniqueParticleFrame != 0)
				color *= 1f/uniqueParticleFrame;
				if(!Main.tile[i, j - 1].active() || !Main.tileSolid[Main.tile[i, j - 1].type])
				{
					ulong randSeed = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (long)((ulong)i));
					for (int l = 0; l < 7; l++)
					{
						float x = (float)Utils.RandomInt(ref randSeed, -16, 17) * 0.1f;
						float y = (float)Utils.RandomInt(ref randSeed, -16, 17) * 0.1f;
						Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y - 2) + zero, null, color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
					}
					//spriteBatch.Draw(texture, drawPos, null, color, 0, new Vector2(0,0), 1, SpriteEffects.None, 0f);
				}
			}
			color = new Color(100, 100, 100, 0);
			ulong randSeed2 = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (long)((ulong)i));
			for (int l = 0; l < 7; l++)
			{
				float x = (float)Utils.RandomInt(ref randSeed2, -16, 17) * 0.1f;
				float y = (float)Utils.RandomInt(ref randSeed2, -16, 17) * 0.1f;
				bool canUp = true;
				bool canDown = true;
				bool canLeft = true;
				bool canRight = true;
				if(Main.tile[i, j - 1].active() && Main.tileSolid[Main.tile[i, j - 1].type])
					canUp = false;
				
				if(Main.tile[i, j + 1].active() && Main.tileSolid[Main.tile[i, j + 1].type])
					canDown = false;
				
				if(Main.tile[i + 1, j].active() && Main.tileSolid[Main.tile[i + 1, j].type])
					canRight = false;
				
				if(Main.tile[i - 1, j].active() && Main.tileSolid[Main.tile[i - 1, j].type])
					canLeft = false;
				
				if(!canUp && !canDown)
				{
					y = 0;
				}
				else if(!canUp || !canDown)
				{
					if(!canUp)
					y = Math.Abs(y);
				
					if(!canDown)
					y = -Math.Abs(y);
				}
				if(!canRight && !canLeft)
				{
					x = 0;
				}
				else if(!canRight || !canLeft)
				{
					if(!canRight)
					x = -Math.Abs(x);
				
					if(!canLeft)
					x = Math.Abs(x);
				}
				Main.spriteBatch.Draw(textureBlock, new Vector2((float)(i * 16 - (int)Main.screenPosition.X) + x, (float)(j * 16 - (int)Main.screenPosition.Y) + y - 2) + zero, 
				new Rectangle(0, 20 * (Main.tile[i, j].halfBrick() ? 1 : Main.tile[i, j].slope() > 0 ? Main.tile[i, j].slope() + 1 : 0), 16, 20), color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
			}
			//spriteBatch.Draw(texture, drawPos, null, color, 0, new Vector2(0,0), 1, SpriteEffects.None, 0f);
			
			return true;
		}
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            Framing.SelfFrame8Way(i, j, Main.tile[i, j], resetFrame);
            return false;
        }
	}
}