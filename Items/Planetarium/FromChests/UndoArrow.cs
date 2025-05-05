using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Chaos;
using Terraria.DataStructures;
using SOTS.Projectiles.AbandonedVillage;

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
			Item.shoot = ModContent.ProjectileType<ExcavatorBolt>(); 
            Item.shootSpeed = 3.0f;
			Item.consumable = true;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			//CollapseBlock.Spawn(source, Main.MouseWorld.ToTileCoordinates().X, Main.MouseWorld.ToTileCoordinates().Y);
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
			//SOTSPlayer sPlayer = player.SOTSPlayer();
			//sPlayer.UniqueVisionNumber++;
			//sPlayer.UniqueVisionNumber = sPlayer.UniqueVisionNumber % 40;
			//player.VoidPlayer().ResetAllVoidBonuses();
			for(int i = 0; i < 1; i++)
			{
				Vector2 target = Main.MouseWorld + Main.rand.NextVector2Circular(640, 640);
				Projectile.NewProjectile(source, position, velocity + Main.rand.NextVector2Circular(1, 1), type, damage, knockback, player.whoAmI, 3);
                //Projectile.NewProjectile(source, position, Main.rand.NextVector2Circular(4, 4), ModContent.ProjectileType<ExcavatorOrb>(), damage, knockback, player.whoAmI);
            }
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