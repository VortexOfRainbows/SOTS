using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using SOTS.Void;
using Terraria.Localization;
using System.Xml;
using System;
using System.Net.Http.Headers;
using System.Net.Sockets;
using SOTS.Dusts;

namespace SOTS.Items.AbandonedVillage
{
	public class VisionAmulet : ModItem
	{
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Player player = Main.LocalPlayer;
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			int unique = modPlayer.UniqueVisionNumber;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Items/AbandonedVillage/VisionAmuletSheet");
			frame = new Rectangle(38 * GetGem(unique), 44 * GetFrame(unique) + 2, 36, 40);
			spriteBatch.Draw(texture, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Player player = Main.LocalPlayer;
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			int unique = modPlayer.UniqueVisionNumber;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Items/AbandonedVillage/VisionAmuletSheet");
			Rectangle frame = new Rectangle(38 * GetGem(unique), 44 * GetFrame(unique) + 2, 36, 40);
			Vector2 origin = Item.Size / 2;
			spriteBatch.Draw(texture, Item.Center - Main.screenPosition, frame, lightColor, rotation, origin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.width = 36;     
            Item.height = 40;   
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
			Item.hasVanityEffects = true;
			Item.shopCustomPrice = Item.buyPrice(1, 0, 0, 0);
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Player player = Main.LocalPlayer;
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			int unique = modPlayer.UniqueVisionNumber;
			foreach (TooltipLine line in tooltips) //goes through each tooltip line
			{
				if (line.Mod == "Terraria" && line.Name == "Tooltip0") //checks the name of the tootip line
				{
					line.Text = GetTooltip(GetGem(unique), GetFrame(unique));
					return;
				}
			}
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			int unique = modPlayer.UniqueVisionNumber;
			GetBonuses(player, GetGem(unique), GetFrame(unique));
			if(!hideVisual)
				modPlayer.VisionVanity = true;
		}
        public static int GetFrame(int unique)
        {
			return unique / 8;
        }
        public static int GetGem(int unique)
		{
			return unique % 8;
		}
        public static void GetBonuses(Player player, int gem, int frame)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			switch (gem)
            {
				case 0: //geo
					player.endurance += 0.1f;
					break;
				case 1: //electro
					player.maxMinions++;
					player.maxTurrets++;
					break;
				case 2: //anemo
					modPlayer.attackSpeedMod += 0.12f;
					break;
				case 3: //cyro
					player.GetCritChance(DamageClass.Generic) += 10;
					break;
				case 4: //pyro
					modPlayer.CritBonusMultiplier += 0.3f;
					break;
				case 5: //hydro
					modPlayer.additionalHeal += 40;
					player.lifeRegen += 2;
					break;
				case 6: //dendro
					player.statLifeMax2 += 20;
                    player.GetDamage(DamageClass.Generic) += 0.1f;
                    break;
				case 7: //masterless
					vPlayer.voidRegenSpeed += 0.2f;
					break;
			}
			switch(frame)
            {
				case 0: //liyue
					player.discountAvailable = true;
					break;
				case 1: //inazuma
					modPlayer.PotionBuffDegradeRate -= 0.2f;
					player.manaCost -= 0.1f;
					break;
				case 2: //mondstadt
					player.jumpSpeedBoost += 2f;
					player.moveSpeed += 0.1f;
					player.GetAttackSpeed(DamageClass.Melee) += 0.1f;
					break;
				case 3: //Sumeru		
					modPlayer.LazyCrafterAmulet = true;
					modPlayer.additionalPotionMana += 40;
					player.statManaMax2 += 40;
					break;
                case 4: //Fontaine
					modPlayer.StatShareAll = true;
                    break;
				case 5: //Natlan
					modPlayer.ScalingArmorPenetration = true;
					break;
				case 6: //Snezhnaya
					vPlayer.voidGainMultiplier += 0.2f;
					player.GetDamage<VoidGeneric>() += 0.1f;
					break;
            }
        }
        public static string GetTooltip(int gem, int frame)
		{
			string text = Language.GetTextValue($"Mods.SOTS.VisionAmuletTextList.{gem}");
			text += Language.GetTextValue($"Mods.SOTS.VisionAmuletTextList2.{frame}");
			return text;
		}
	}
	public class VisionAmuletSwitchAnimation : ModProjectile
	{
        public override string Texture => "SOTS/Items/AbandonedVillage/VisionAmuletSheet";
        public int PreviousAmulet => (int)Projectile.ai[0];
		public int NewAmulet => (int)Projectile.ai[1];
		public float AI0 = 0;
        public override void SetStaticDefaults()
        {

        }
        public override void SetDefaults()
        {
			Projectile.Size = new Vector2(36, 40);
			Projectile.friendly = Projectile.hostile = false;
			Projectile.timeLeft = 300;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
			Texture2D t = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
            Rectangle frame = new Rectangle(38 * VisionAmulet.GetGem(PreviousAmulet), 44 * VisionAmulet.GetFrame(PreviousAmulet) + 2, 36, 40);
            Rectangle newframe = new Rectangle(38 * VisionAmulet.GetGem(NewAmulet), 44 * VisionAmulet.GetFrame(NewAmulet) + 2, 36, 40);
            Vector2 origin = frame.Size() / 2;

            float percent = MathHelper.Clamp(AI0 / 40f, 0, 1);
			if(AI0 > 140)
			{
				percent = 1 - ((AI0 - 140) / 40f);
			}
			float transformPercent = (AI0 - 40f) / 60f;
			if (transformPercent <= 1)
            {
                if (transformPercent > 0)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        Vector2 circular = new Vector2(38 * (1 - transformPercent), 0).RotatedBy(i * MathF.PI / 3f + MathF.PI * transformPercent);
                        Main.EntitySpriteDraw(t, Projectile.Center - Main.screenPosition + circular, newframe, new Color(100, 100, 100, 0) * transformPercent * transformPercent, 0, origin, 1f, SpriteEffects.None, 0f);
                    }
                }
            }
			else
			{
				frame = newframe;
			}
            Main.EntitySpriteDraw(t, Projectile.Center - Main.screenPosition, frame, Color.White * percent, 0, origin, 1f, SpriteEffects.None, 0f);

            return false;
        }
		private float traversedDist = 0.0f;
        public override void AI()
        {
			Player p = Main.player[(int)Projectile.ai[2]];
			Projectile.Center = new Vector2(p.Center.X, p.Center.Y + traversedDist);
			Projectile.velocity.Y *= 0.94f; 
            traversedDist += Projectile.velocity.Y;
            if (AI0 == 40)
			{
				SOTSUtils.PlaySound(SoundID.Item15, Projectile.Center, 1, 0.1f);
			}
			AI0++;
			if(AI0 >= 100)
			{
				if(AI0 == 100)
				{
                    SOTSUtils.PlaySound(SoundID.Item4, Projectile.Center, 1,-0.4f);
                    Color c = SOTSPlayer.VisionColorFromNumber(NewAmulet);
                    c.A = 0;
                    for (int i = 0; i < 50; ++i)
                    {
						Vector2 circular = new Vector2(6, 0).RotatedBy(MathHelper.TwoPi * i / 50f);
                        float scale = Main.rand.NextFloat(1.5f, 2.5f);
                        Dust d = PixelDust.Spawn(Projectile.Center, 0, 0, circular / scale, c, 3);
                        d.scale = scale;
                    }
                }
            }
			if(AI0 >= 180)
			{
				Projectile.Kill();
			}
        }
    }
}

