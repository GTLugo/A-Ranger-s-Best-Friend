using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BestFriend.Content.Buffs;

public class BlankDebuff : ModBuff {
  public override string Texture => BestFriend.ASSET_PATH + "Textures/Buffs/BuffTemplate";

  public override void SetStaticDefaults() {
    // DisplayName.SetDefault("Bullet of Love");
    // Description.SetDefault("You lose");


    Main.debuff[Type] = true;
    Main.pvpBuff[Type] = true;
    Main.buffNoSave[Type] = true;
    BuffID.Sets.IsATagBuff[Type] = true;
    BuffID.Sets.LongerExpertDebuff[Type] = false;
  }

  public override void Update(NPC npc, ref int buffIndex) {
    npc.GetGlobalNPC<BlankDebuffNPC>().blankDebuff = true;
  }

  public override void Update(Player player, ref int buffIndex) {
    player.GetModPlayer<BlankDebuffPlayer>().blankDebuff = true;
  }
}

public class BlankDebuffPlayer : ModPlayer {
  public bool blankDebuff;

  public override void ResetEffects() {
    blankDebuff = false;
  }

  public override void UpdateBadLifeRegen() {
    if (blankDebuff) {
      if (Player.lifeRegen > 0) {
        Player.lifeRegen = 0;
      }

      Player.lifeRegenTime = 0;

      // lifeRegen is measured in 1/2 life per second.
      Player.lifeRegen -= 2;
    }
  }
}

public class BlankDebuffNPC : GlobalNPC {
  public bool blankDebuff;
  public override bool InstancePerEntity => true;

  public override void ResetEffects(NPC npc) {
    blankDebuff = false;
  }

  public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers) { }

  public override void UpdateLifeRegen(NPC npc, ref int damage) {
    if (blankDebuff) {
      if (npc.lifeRegen > 0) {
        npc.lifeRegen = 0;
      }

      // lifeRegen is measured in 1/2 life per second.
      npc.lifeRegen -= 2;
      damage = 1;
    }
  }
}