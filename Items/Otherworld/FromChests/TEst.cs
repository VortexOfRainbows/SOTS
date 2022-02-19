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

namespace SOTS.Items.Otherworld.FromChests
{
	public class UndoArrow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("test");
			Tooltip.SetDefault("hi there");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.ThrowingKnife);
			item.damage = 17;
			//item.useTime = 3;
			item.thrown = true;
			item.rare = ItemRarityID.Green;
			item.autoReuse = false;            
			item.shoot = ModContent.ProjectileType<ChaosDiamond>(); 
            item.shootSpeed = 3.0f;
			item.consumable = true;
		}
		int counter = 0;
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			//DrawTexture();
			//VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			//vPlayer.voidStar = 0;
			//vPlayer.voidAnkh = 0;
			//vPlayer.voidMeterMax = 100;
			//Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, Main.myPlayer, Main.rand.NextFloat(0.65f, 0.75f), Main.rand.NextFloat(360));
			////SOTSPlayer.ModPlayer(player).UniqueVisionNumber = (SOTSPlayer.ModPlayer(player).UniqueVisionNumber + 1) % 24;
			position = Main.MouseWorld;
			return true; 
		}
		public void DrawTexture()
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
		}
	}
}