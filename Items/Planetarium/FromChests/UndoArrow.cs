using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Projectiles.Celestial;
using SOTS.Projectiles.Inferno;
using SOTS.NPCs.Boss.Curse;
using SOTS.Projectiles.Pyramid;
using SOTS.Buffs;
using SOTS.Projectiles.Slime;
using SOTS.Void;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using SOTS.Projectiles.Chaos;
using Terraria.DataStructures;
using Terraria.Localization;

namespace SOTS.Items.Planetarium.FromChests
{
	public class UndoArrow : ModItem
	{
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Item[Item.type].Value;
            spriteBatch.Draw(texture, position, frame, Color.White, 0f, origin, scale, SpriteEffects.None, 0f);
            return false;
        }
        public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.ThrowingKnife);
			Item.damage = 17;
			//Item.useTime = 3;
			Item.DamageType = DamageClass.Throwing;
			Item.rare = ItemRarityID.Green;
			Item.autoReuse = false;            
			Item.shoot = ModContent.ProjectileType<ThunderBall>(); 
            Item.shootSpeed = 3.0f;
			Item.consumable = true;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			//for(int i = 0; i < 200; i ++)
			//{
			//	if(Main.npc[i].active)
			//		Main.npc[i].aiStyle = -420;
			// }
			//Main.NewText(Main.invasionType);
			//Main.NewText(Language.GetTextValue("Mods.SOTS.MapObject.LockedStrangeChest"));
			/*if (SOTSWorld.DiamondKeySlotted && SOTSWorld.RubyKeySlotted
				&& SOTSWorld.EmeraldKeySlotted && SOTSWorld.SapphireKeySlotted
				&& SOTSWorld.TopazKeySlotted && SOTSWorld.AmethystKeySlotted && SOTSWorld.AmberKeySlotted)
			{
				SOTSWorld.RubyKeySlotted = false;
				SOTSWorld.EmeraldKeySlotted = false;
				SOTSWorld.SapphireKeySlotted = false;
				SOTSWorld.DiamondKeySlotted = false;
				SOTSWorld.AmberKeySlotted = false;
				SOTSWorld.TopazKeySlotted = false;
				SOTSWorld.AmethystKeySlotted = false;
			}
			else
            {
				int next = Main.rand.Next(7);
				if(next == 0)
					SOTSWorld.RubyKeySlotted = true;
				if (next == 1)
					SOTSWorld.EmeraldKeySlotted = true;
				if (next == 2)
					SOTSWorld.SapphireKeySlotted = true;
				if (next == 3)
					SOTSWorld.DiamondKeySlotted = true;
				if (next == 4)
					SOTSWorld.TopazKeySlotted = true;
				if (next == 5)
					SOTSWorld.AmethystKeySlotted = true;
				if (next == 6)
					SOTSWorld.AmberKeySlotted = true;
			}*/
			SOTSPlayer sPlayer = player.sotsPlayer();
			sPlayer.UniqueVisionNumber++;
			sPlayer.UniqueVisionNumber = sPlayer.UniqueVisionNumber % 32;
			player.VoidPlayer().ResetAllVoidBonuses();
			//Projectile.NewProjectile(position, new Vector2(0, 1), ModContent.ProjectileType<ThunderSpawnBeam>(), 0, 0, Main.myPlayer, Main.MouseWorld.X, Main.MouseWorld.Y);
			return false; 
		}
		/*public void DrawTexture()
        {
			Texture2D texture = new Texture2D(Main.spriteBatch.GraphicsDevice, 800, 800, false, SurfaceFormat.Color);
			System.Collections.Generic.List<Color> list = new System.Collections.Generic.List<Color>();
			for (int i = 0; i < texture.Width; i++)
			{
				for (int j = 0; j < texture.Height; j++)
				{
					float x = (2 * i / (float)(texture.Width - 1) - 1);
					float y = (2 * j / (float)(texture.Width - 1) - 1);

					float distanceSquared = x * x + y * y;
					float theta = new Vector2(x, y).ToRotation();
					float cos = (float)Math.Cos(4 * theta + 12 * Math.Pow(distanceSquared, 0.9));
					float twistyFactor = (float)(((1 + cos) / 2) * Math.Sqrt(distanceSquared));
					float scaleFactor = (float)(1 - Math.Sqrt(distanceSquared)) * (twistyFactor - 1) + 1;

					int alpha = distanceSquared >= 1 ? 0 : (int)(205 * (1 - scaleFactor) * (0.5f - distanceSquared + (float)Math.Abs(cos)) + (50 * (1 - distanceSquared)));

					list.Add(new Color(alpha, alpha, alpha));
				}
			}
			texture.SetData(list.ToArray());
			texture.SaveAsPng(new FileStream(Main.SavePath + Path.DirectorySeparatorChar + "TestEffect.png", FileMode.Create), texture.Width, texture.Height);
		}*/
	}
}