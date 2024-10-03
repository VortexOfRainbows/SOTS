using Microsoft.Xna.Framework;
using SOTS.Projectiles.Planetarium;
using SOTS.Void;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Planetarium
{
	public class TwilightBeads : VoidItem	
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
			Item.damage = 34;
			Item.maxStack = 1;
            Item.width = 30;     
            Item.height = 26;   
            Item.value = Item.sellPrice(0, 0, 80, 0);
            Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer voidPlayer = player.GetModPlayer<VoidPlayer>();
			voidPlayer.bonusVoidGain += 1f;
			BeadPlayer modPlayer = player.GetModPlayer<BeadPlayer>();
			modPlayer.soulDamage += SOTSPlayer.ApplyDamageClassModWithGeneric(player, Item.DamageType, Item.damage);
			modPlayer.RetaliationSouls = true;
		}
	}
	public class BeadPlayer : ModPlayer
	{
		public int attackNum = 0;
		public int soulDamage = 0;
		public bool RetaliationSouls = false;
		public override void ResetEffects()
		{
            if (attackNum >= 10)
			{
				attackNum++;
				if(attackNum > 12)
				{
					attackNum = 0;
				}
			}
			soulDamage = 0;
			RetaliationSouls = false;
		}
		public void SpawnSouls()
		{
			if (attackNum < 10 && Player.whoAmI == Main.myPlayer && Player.ownedProjectileCounts[ModContent.ProjectileType<SoulofRetaliation>()] < 5 && RetaliationSouls)
			{
				for (int i = 0; i < 5; i++)
				{
					Projectile.NewProjectile(Player.GetSource_Misc("SOTS:TwilightBeads"), Player.Center, Vector2.Zero, ModContent.ProjectileType<SoulofRetaliation>(), soulDamage, 1f, Main.myPlayer);
				}
			}
		}
        public override void OnHurt(Player.HurtInfo info)
        {
            SpawnSouls();
        }
	}
}