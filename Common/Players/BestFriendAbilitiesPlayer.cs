using System.Diagnostics.CodeAnalysis;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BestFriend.Common.Players;

public class BestFriendAbilitiesPlayer : ModPlayer {
  // public const int MAX_SERUMS = 1;
  public bool has_serum = false;

  public bool hit_scan = false;

  public override void ResetEffects() {
    hit_scan = false;

    var mod_player = Player.GetModPlayer<BestFriendAbilitiesPlayer>();

    if (mod_player.has_serum) {
      Player.moveSpeed += 0.1f;
      Player.jumpSpeedBoost += 1.2f;
      Player.lifeRegen += 2;
      Player.statDefense += 4;
      Player.pickSpeed -= 0.15f;

      Player.GetAttackSpeed(DamageClass.Generic) += 0.1f;
      Player.GetDamage(DamageClass.Generic) += 0.1f;
      Player.GetCritChance(DamageClass.Generic) += 2;
      Player.GetKnockback(DamageClass.Generic) += 0.1f;

      // Based upon documentation for tModloader, it doesn't seem like DamageClass.Generic applies to DamageClass.Throwing
      Player.GetAttackSpeed(DamageClass.Throwing) += 0.1f;
      Player.GetDamage(DamageClass.Throwing) += 0.1f;
      Player.GetCritChance(DamageClass.Throwing) += 2;
      Player.GetKnockback(DamageClass.Throwing) += 0.1f;
    }

    base.ResetEffects();
  }

  public override void SyncPlayer(int toWho, int fromWho, bool newPlayer) {
    // REMEMBER TO ADD PACKET READING TO RangersArsenal.cs
    var packet = Mod.GetPacket();
    packet.Write((byte)BestFriend.BestFriendModMessageType.SyncPlayer);
    packet.Write((byte)Player.whoAmI);
    packet.Write(has_serum);
    packet.Write(hit_scan);
    packet.Send(toWho, fromWho);
  }

  public override void ModifyMaxStats([UnscopedRef] out StatModifier health, [UnscopedRef] out StatModifier mana) {
    base.ModifyMaxStats(out health, out mana);

    var mod_player = Player.GetModPlayer<BestFriendAbilitiesPlayer>();

    if (mod_player.has_serum) {
      health.Base += 100;
    }
  }

  public override void CopyClientState(ModPlayer targetCopy) {
    var clone = (BestFriendAbilitiesPlayer)targetCopy;
    clone.has_serum = has_serum;
    clone.hit_scan = hit_scan;
  }

  public override void SendClientChanges(ModPlayer clientPlayer) {
    BestFriendAbilitiesPlayer clone = (BestFriendAbilitiesPlayer)clientPlayer;

    if (has_serum != clone.has_serum || hit_scan != clone.hit_scan)
      SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
  }

  public override void SaveData(TagCompound tag) {
    tag["has_serum"] = has_serum;
    tag["hit_scan"] = hit_scan;
  }

  public override void LoadData(TagCompound tag) {
    has_serum = tag.GetBool("has_serum");
    hit_scan = tag.GetBool("hit_scan");
  }
}