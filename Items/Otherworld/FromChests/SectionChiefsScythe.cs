using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.Otherworld.FromChests
{
	public class SectionChiefsScythe : VoidItem
	{ 	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Section Chief's Scythe");
			Tooltip.SetDefault("Critical hits summon a Soul of Retaliation into the air\nEvery 10th void attack will release the soul in the form of a powerful laser\nDirect melee hits permanently curse enemies for 4 damage per second, stacking up to 10 times");
		}
		public override void SafeSetDefaults()
		{
            item.damage = 48;  
            item.melee = true; 
            item.width = 58;    
            item.height = 58;  
            item.useTime = 24;
            item.useAnimation = 24;
            item.useStyle = 1;   
            item.autoReuse = true; 
            item.knockBack = 3f;
			item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = ItemRarityID.LightPurple;
            item.UseSound = SoundID.Item71;
			item.crit = 11;
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/Otherworld/FromChests/HardlightScytheGlow");
			}

			item.shoot = mod.ProjectileType("ScytheSlash");
			item.shootSpeed = 15f;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Otherworld/FromChests/HardlightScytheGlow");
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.NextBool(10))
			{
				Dust dust = Dust.NewDustDirect(hitbox.Location.ToVector2() - new Vector2(5f), hitbox.Width, hitbox.Height, mod.DustType("CopyDust4"), 0, -2, 200, new Color(), 1f);
				dust.velocity *= 0.4f;
				dust.color = new Color(100, 100, 255, 120);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 1.5f;
			}
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			knockBack *= 3f;
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }
        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
		{
			if(crit)
			{
				BeadPlayer modPlayer = player.GetModPlayer<BeadPlayer>();
				if (crit && Main.myPlayer == player.whoAmI)
				{
					Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, mod.ProjectileType("SoulofRetaliation"), damage + modPlayer.soulDamage, 1f, player.whoAmI);
				}
			}
		}
		public override void GetVoid(Player player)
		{
			voidMana = 10;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PlatinumScythe", 1);
			recipe.AddIngredient(null, "HardlightAlloy", 12);
			recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
