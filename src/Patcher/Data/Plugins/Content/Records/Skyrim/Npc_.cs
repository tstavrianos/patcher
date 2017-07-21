/// Copyright(C) 2015 Unforbidable Works
///
/// This program is free software; you can redistribute it and/or
/// modify it under the terms of the GNU General Public License
/// as published by the Free Software Foundation; either version 2
/// of the License, or(at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
/// GNU General Public License for more details.
///
/// You should have received a copy of the GNU General Public License
/// along with this program; if not, write to the Free Software
/// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

using Patcher.Data.Plugins.Content.Fields.Skyrim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Data.Plugins.Content.Records.Skyrim
{
    [Record(Names.NPC_)]
    [Game(Games.Skyrim)]
    public sealed class Npc_ : GenericFormRecord, IFeaturingObjectBounds, IFeaturingScripts
    {
        [Member(Names.VMAD)]
        public VirtualMachineAdapter VirtualMachineAdapter { get; set; }
        
        [Member(Names.OBND)]
        [Initialize]
        public ObjectBounds ObjectBounds { get; set; }

        [Member(Names.ACBS)]
        public NpcConfiguration Configuration {get; set;}
        
        [Member(Names.SNAM)]
        [Initialize]
        public List<FactionField> Factions {get; set;}

        [Member(Names.INAM)]
        [Reference(Names.LVLI)]
        public uint DeathItem { get; set; }

        [Member(Names.VTCK)]
        [Reference(Names.VTYP)]
        public uint Voice { get; set; }

        [Member(Names.TPLT)]
        [Reference(Names.LVLN + Names.NPC_)]
        public uint Template { get; set; }

        [Member(Names.RNAM)]
        [Reference(Names.RACE)]
        public uint Race { get; set; }

        [Member(Names.SPCT)]
        public uint ActorEffectCount { get; set; }

        [Member(Names.SPLO)]
        [Reference(Names.SPEL + Names.SHOU + Names.LVSP)]
        [Initialize]
        public List<uint> ActorEffects { get; set; }
        
        [Member(Names.DEST)]
        public DestructionData DestructionData { get; set; }

        [Member(Names.WNAM)]
        [Reference(Names.ARMO)]
        public uint WornArmor { get; set; }

        [Member(Names.ANAM)]
        [Reference(Names.ARMO)]
        public uint FarAwayModel { get; set; }

        [Member(Names.ATKR)]
        [Reference(Names.RACE)]
        public uint AttackRace { get; set; }

        [Member(Names.ATKD)]
        [Initialize]
        public List<AttackDataField> AttackData { get; set; }

        [Member(Names.SPOR)]
        [Reference(Names.FLST)]
        public uint SpectatorOverridePackageList { get; set; }

        [Member(Names.OCOR)]
        [Reference(Names.FLST)]
        public uint ObserveDeadBodyOverridePackageList { get; set; }

        [Member(Names.GWOR)]
        [Reference(Names.FLST)]
        public uint GuardWarnOverridePackageList { get; set; }

        [Member(Names.ECOR)]
        [Reference(Names.FLST)]
        public uint CombatOverridePackageList { get; set; }
        
        [Member(Names.PRKZ)]
        public uint PerkCount { get; set; }
        
        [Member(Names.PRKR)]
        [Initialize]
        public List<PerkField> Perks {get; set;}

        [Member(Names.COCT)]
        public uint ItemCount { get; set; }

        
        //CNTO
        //AIDT
        [Member(Names.PACK)]
        [Reference(Names.PKID)]
        [Initialize]
        public List<uint> Packages { get; set; }

        //KSIZ
        //KWDA

        [Member(Names.CNAM)]
        [Reference(Names.CLAS)]
        public uint Class { get; set; }
        
        //FULL

        [Member(Names.SHRT)]
        [LocalizedString(LocalizedStringGroups.Strings)]
        public string ShortName { get; set; }

        [Member(Names.DATA)]
        public Fields.ByteArray Marker { get; set; }

        [Member(Names.DNAM)]
        public DNAMField PlayerSkills { get; set; }

        [Member(Names.PNAM)]
        [Reference(Names.HDPT)]
        [Initialize]
        public List<uint> HeadParts { get; set; }

        [Member(Names.HCLF)]
        [Reference(Names.CLFM)]
        public uint HairColor { get; set; }

        [Member(Names.ZNAM)]
        [Reference(Names.CSTY)]
        public uint CombatStyle { get; set; }

        [Member(Names.GNAM)]
        [Reference(Names.FLST)]
        public uint GiftFilter { get; set; }

        public sealed class NpcConfiguration: Field
        {
            public uint Flags {get; set;}
            public short MagickaOffset {get; set;}
            public short StaminaOffset {get; set;}
            public short Level {get; set;}
            public ushort CalcMinLevel {get; set;}
            public ushort CalcMaxLevel {get; set;}
            public ushort SpeedMultiplier {get; set;}
            public short DispositionBase {get; set;}
            public ushort TemplateFlags {get; set;}
            public short HealthOffset {get; set;}
            public ushort BleedoutOverride {get; set;}

            internal override void ReadField(RecordReader reader)
            {
                Flags = reader.ReadUInt32();
                MagickaOffset = reader.ReadInt16();
                StaminaOffset = reader.ReadInt16();
                Level = reader.ReadInt16();
                CalcMinLevel = reader.ReadUInt16();
                CalcMaxLevel = reader.ReadUInt16();
                SpeedMultiplier = reader.ReadUInt16();
                DispositionBase = reader.ReadInt16();
                TemplateFlags = reader.ReadUInt16();
                HealthOffset = reader.ReadInt16();
                BleedoutOverride = reader.ReadUInt16();
            }

            internal override void WriteField(RecordWriter writer)
            {
                writer.Write(Flags);
                writer.Write(MagickaOffset);
                writer.Write(StaminaOffset);
                writer.Write(Level);
                writer.Write(CalcMinLevel);
                writer.Write(CalcMaxLevel);
                writer.Write(SpeedMultiplier);
                writer.Write(DispositionBase);
                writer.Write(TemplateFlags);
                writer.Write(HealthOffset);
                writer.Write(BleedoutOverride);
            }

            public override Field CopyField()
            {
                return new NpcConfiguration()
                {
                    Flags = this.Flags,
                    MagickaOffset = this.MagickaOffset,
                    StaminaOffset = this.StaminaOffset,
                    Level = this.Level,
                    CalcMinLevel = this.CalcMinLevel,
                    CalcMaxLevel = this.CalcMaxLevel,
                    SpeedMultiplier = this.SpeedMultiplier,
                    DispositionBase = this.DispositionBase,
                    TemplateFlags = this.TemplateFlags,
                    HealthOffset = this.HealthOffset,
                    BleedoutOverride = this.BleedoutOverride,
                };
            }

            public override bool Equals(Field other)
            {
                var cast = (NpcConfiguration)other;

                return cast != null &&
                    cast.Flags == this.Flags &&
                    cast.MagickaOffset == this.MagickaOffset &&
                    cast.StaminaOffset == this.StaminaOffset &&
                    cast.Level == this.Level &&
                    cast.CalcMinLevel == this.CalcMinLevel &&
                    cast.CalcMaxLevel == this.CalcMaxLevel &&
                    cast.SpeedMultiplier == this.SpeedMultiplier &&
                    cast.DispositionBase == this.DispositionBase &&
                    cast.TemplateFlags == this.TemplateFlags &&
                    cast.HealthOffset == this.HealthOffset &&
                    cast.BleedoutOverride == this.BleedoutOverride;
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                return string.Format("ACBS {0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10}", Flags, MagickaOffset, StaminaOffset, Level, CalcMinLevel, CalcMaxLevel, SpeedMultiplier, DispositionBase, TemplateFlags, HealthOffset, BleedoutOverride);
            }
        }

        public sealed class FactionField: Field
        {
            public uint Faction {get; set;}
            public byte Rank {get; set;}
            public byte[] Unused {get; set;}

            public override Field CopyField()
            {
                return new FactionField()
                {
                    Faction = this.Faction,
                    Rank = this.Rank,
                    Unused = this.Unused
                };
            }

            public override bool Equals(Field other)
            {
                var cast = (FactionField)other;
                return cast != null &&
                    cast.Faction != Faction &&
                    cast.Rank != Rank &&
                    cast.Unused != Unused;
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                yield return Faction;
            }

            public override string ToString()
            {
                throw new NotImplementedException();
            }

            internal override void ReadField(RecordReader reader)
            {
                Faction = reader.ReadReference(FormKindSet.FactOnly);
                Rank = reader.ReadByte();
                Unused = reader.ReadBytes(3);
            }

            internal override void WriteField(RecordWriter writer)
            {
                writer.WriteReference(Faction, FormKindSet.FactOnly);
                writer.Write(Rank);
                writer.Write(Unused);
            }
        }

        public sealed class PerkField: Field
        {
            public uint Perk { get; set; }
            public byte Rank { get; set; }
            public byte[] Unused { get; set; }

            public override Field CopyField()
            {
                return new PerkField()
                {
                    Perk = this.Perk,
                    Rank = this.Rank,
                    Unused = this.Unused
                };
            }

            public override bool Equals(Field other)
            {
                var cast = (PerkField)other;
                return cast != null &&
                    cast.Perk != Perk &&
                    cast.Rank != Rank &&
                    cast.Unused != Unused;
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                yield return Perk;
            }

            public override string ToString()
            {
                throw new NotImplementedException();
            }

            internal override void ReadField(RecordReader reader)
            {
                Perk = reader.ReadReference(FormKindSet.PerkOnly);
                Rank = reader.ReadByte();
                Unused = reader.ReadBytes(3);
            }

            internal override void WriteField(RecordWriter writer)
            {
                writer.WriteReference(Perk, FormKindSet.PerkOnly);
                writer.Write(Rank);
                writer.Write(Unused);
            }
        }

        public sealed class DNAMField : Field
        {
            public byte[] SkillValues { get; set; }
            public byte[] SkillOffsets { get; set; }
            public ushort Health { get; set; }
            public ushort Magicka { get; set; }
            public ushort Stamina { get; set; }
            public ushort Unused1 { get; set; }
            public float FarAwayModelDistance { get; set; }
            public byte GearedUpWeapons { get; set; }
            public byte[] Unused2 { get; set; }

            public override Field CopyField()
            {
                return new DNAMField()
                {
                    SkillValues = this.SkillValues,
                    SkillOffsets = this.SkillOffsets,
                    Health = this.Health,
                    Magicka = this.Magicka,
                    Stamina = this.Stamina,
                    Unused1 = this.Unused1,
                    FarAwayModelDistance = this.FarAwayModelDistance,
                    GearedUpWeapons = this.GearedUpWeapons,
                    Unused2 = this.Unused2
                };
            }

            public override bool Equals(Field other)
            {
                var cast = (DNAMField)other;
                return cast != null &&
                    cast.SkillValues == SkillValues &&
                    cast.SkillOffsets == SkillOffsets &&
                    cast.Health == Health &&
                    cast.Magicka == Magicka &&
                    cast.Stamina == Stamina &&
                    cast.Unused1 == Unused1 &&
                    cast.FarAwayModelDistance == FarAwayModelDistance &&
                    cast.GearedUpWeapons == GearedUpWeapons &&
                    cast.Unused2 == Unused2;
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                throw new NotImplementedException();
            }

            internal override void ReadField(RecordReader reader)
            {
                SkillValues = reader.ReadBytes(18);
                SkillOffsets = reader.ReadBytes(18);
                Health = reader.ReadUInt16();
                Magicka = reader.ReadUInt16();
                Stamina = reader.ReadUInt16();
                Unused1 = reader.ReadUInt16();
                FarAwayModelDistance = reader.ReadSingle();
                GearedUpWeapons = reader.ReadByte();
                Unused2 = reader.ReadBytes(3);
            }

            internal override void WriteField(RecordWriter writer)
            {
                writer.Write(SkillValues);
                writer.Write(SkillOffsets);
                writer.Write(Health);
                writer.Write(Magicka);
                writer.Write(Stamina);
                writer.Write(Unused1);
                writer.Write(FarAwayModelDistance);
                writer.Write(GearedUpWeapons);
                writer.Write(Unused2);
            }
        }

        public sealed class AttackDataField: Field
        {
            public float DamageMult {get; set;}
            public float AttackChance {get; set;}
            public uint AttackSpell {get; set;}
            public uint Flags {get; set;}
            public float AttackAngle {get; set;}
            public float StrikeAngle {get; set;}
            public float Stagger {get; set;}
            public uint AttackType {get; set;}
            public float Knockdown {get; set;}
            public float RecoveryTime {get; set;}
            public float FatigueMult {get; set;}

            public override Field CopyField()
            {
                return new AttackDataField()
                {
                    DamageMult = this.DamageMult,
                    AttackChance = this.AttackChance,
                    AttackSpell = this.AttackSpell,
                    Flags = this.Flags,
                    AttackAngle = this.AttackAngle,
                    StrikeAngle = this.StrikeAngle,
                    Stagger = this.Stagger,
                    AttackType = this.AttackType,
                    Knockdown = this.Knockdown,
                    RecoveryTime = this.RecoveryTime,
                    FatigueMult = this.FatigueMult
                };
            }

            public override bool Equals(Field other)
            {
                var cast = (AttackDataField)other;
                return cast != null &&
                    cast.DamageMult == this.DamageMult &&
                    cast.AttackChance == this.AttackChance &&
                    cast.AttackSpell == this.AttackSpell &&
                    cast.Flags == this.Flags &&
                    cast.AttackAngle == this.AttackAngle &&
                    cast.StrikeAngle == this.StrikeAngle &&
                    cast.Stagger == this.Stagger &&
                    cast.AttackType == this.AttackType &&
                    cast.Knockdown == this.Knockdown &&
                    cast.RecoveryTime == this.RecoveryTime &&
                    cast.FatigueMult == this.FatigueMult;
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                yield return AttackSpell;
                yield return AttackType;
            }

            public override string ToString()
            {
                throw new NotImplementedException();
            }

            internal override void ReadField(RecordReader reader)
            {
                DamageMult = reader.ReadSingle();
                AttackChance = reader.ReadSingle();
                AttackSpell = reader.ReadReference(FormKindSet.FromNames(Names.SPEL+Names.SHOU+Names.NULL));
                Flags = reader.ReadUInt32();
                AttackAngle = reader.ReadSingle();
                StrikeAngle = reader.ReadSingle();
                Stagger = reader.ReadSingle();
                AttackType = reader.ReadReference(FormKindSet.FromNames(Names.KYWD+Names.NULL));
                Knockdown = reader.ReadSingle();
                RecoveryTime = reader.ReadSingle();
                FatigueMult = reader.ReadSingle();
            }

            internal override void WriteField(RecordWriter writer)
            {
                writer.Write(DamageMult);
                writer.Write(AttackChance);
                writer.WriteReference(AttackSpell, FormKindSet.FromNames(Names.SPEL+Names.SHOU+Names.NULL));
                writer.Write(Flags);
                writer.Write(AttackAngle);
                writer.Write(StrikeAngle);
                writer.Write(Stagger);
                writer.WriteReference(AttackType, FormKindSet.FromNames(Names.KYWD+Names.NULL));
                writer.Write(Knockdown);
                writer.Write(RecoveryTime);
                writer.Write(FatigueMult);
            }
        }

        public sealed class CNTOField: Field
        {
            [Member(Names.CNTO)]
            public CNTOFieldCount Item {get; set; }

            [Member(Names.COED)]
            public COEDField Owner {get; set; }
        }

        public sealed class CNTOFieldCount: Field
        {
            public uint Item;
            public int Count;
        }

        public sealed class COEDField: Field
        {
            public uint Owner;
            public uint Global;
            public int RequiredRank;
            public float ItemCondition;

            public override Field CopyField()
            {
                throw new NotImplementedException();
            }

            public override bool Equals(Field other)
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<uint> GetReferencedFormIds()
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                throw new NotImplementedException();
            }

            internal override void ReadField(RecordReader reader)
            {
                throw new NotImplementedException();
            }

            internal override void WriteField(RecordWriter writer)
            {
                throw new NotImplementedException();
            }
        }
    }
}
